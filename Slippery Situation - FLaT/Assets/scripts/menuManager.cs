using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{

    [Header("main ui page")]
    public GameObject uiPage;
    public TMP_Text titleText;
    public TMP_Text wordText;

    [Header("warning page")]
    public GameObject warningPage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            saveSystem.resetSave("taylor n leah");
        }
    }
    public void openAbout()
    {
        titleText.text = "About Page";
        wordText.text = "insert about description here";
    }

    public void openCredits()
    {
        titleText.text = "Credits Page";
        wordText.text = "insert credits description here";
    }

    public void loadSavedScene()
    {
        sceneData data = saveSystem.loadScene();
        if (data != null && data.currentScene!="taylor n leah")
        {
            SceneManager.LoadScene(data.currentScene);
        }else if (data.currentScene=="taylor n leah")
        {
            print("its the first one");
        }
        else if (data==null)
        {
            print("its null");
        }
    }

    public void newGame()
    {
        //saveSystem.resetSave("Level1");
        saveSystem.resetSave("taylor n leah");
        //SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("taylor n leah");
    }
}
