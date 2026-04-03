using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class saveSystem
{
    public static void saveScene()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sceneProgress";
        FileStream stream = new FileStream(path, FileMode.Create);
        sceneData data = new sceneData();
        formatter.Serialize(stream, data);
        stream.Close(); 
    }

    public static sceneData loadScene()
    {
        string path = Application.persistentDataPath + "/player.sceneProgress";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            sceneData data = formatter.Deserialize(stream) as sceneData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save scene not found" + path);
            return null;
        }
    }

    public static void resetSave(string startScene)
    {
        string path = Application.persistentDataPath + "/player.sceneProgress";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        sceneData data = new sceneData();
        data.currentScene = startScene;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

}
