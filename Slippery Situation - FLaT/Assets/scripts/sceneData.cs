using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class sceneData
{
    public string currentScene;  

    public sceneData()
    {
        currentScene= SceneManager.GetActiveScene().name;
    }
}



