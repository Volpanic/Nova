using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SaveGame
{
    public const string fileName = "save.sav";

    public static void Save(GameSaveData saveToSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath,fileName);

        FileStream stream = new FileStream(path,FileMode.Create);

        formatter.Serialize(stream, saveToSave);
        stream.Close();
    }

    public static GameSaveData LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameSaveData save = formatter.Deserialize(stream) as GameSaveData;
            stream.Close();

            return save;
        }
        else
        {
            return null;
        }
    }
}

[System.Serializable]
public class GameSaveData
{
    public float playerXPos = 0;
    public float playerYPos = 0;
    public string sceneName = "";

    public GameSaveData(float xPos,float yPos ,PlayerController player)
    {
        playerXPos = xPos;
        playerYPos = yPos;
        sceneName = SceneManager.GetActiveScene().name;
    }
}
