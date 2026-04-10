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
            saveSystem.resetSave("Level1");
        }
    }
    public void openAbout()
    {
        titleText.text = "About Page";
        wordText.text = "One frigid night, a freshly frozen ice cube broke free from their tray with a dream of adventuring into the outside world. But little did their solid form know, it had left a watery trail behind on their journey. The world wasn't as cold as they thought it would be...";
    }

    public void openCredits()
    {
        titleText.text = "Credits Page";
    }

    public void loadSavedScene()
    {
        TMP_Text loadText=GameObject.Find("noLoadText").GetComponent<TMP_Text>();
        Animator loadAnim = loadText.gameObject.GetComponent<Animator>();

        sceneData data = saveSystem.loadScene();
        if (data != null && data.currentScene!="Level1")
        {
            SceneManager.LoadScene(data.currentScene);
        }else if (data.currentScene=="Level1")
        {
            print("its the first one");
            loadAnim.SetTrigger("show");
            loadText.color = Color.red;
            loadText.text = "The Loaded Save is on the first level, just start a new game bro";
        }
        else if (data==null)
        {
            print("its null");
            loadAnim.SetTrigger("show");
            loadText.color = Color.red;
            loadText.text = "There is no save to load! Go start a game to save one.";
        }
    }

    public void newGame()
    {
        saveSystem.resetSave("Level1");
        SceneManager.LoadScene("Level1");
    }
}
