using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "menu")
        {
            saveSystem.saveScene();
            print("Scene saved: " + scene.name);
        }
    }
}
