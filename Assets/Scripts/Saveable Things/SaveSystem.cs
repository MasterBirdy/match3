
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveHighScore (HighScoreData highScoreData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.kev";
        FileStream stream = new FileStream(path, FileMode.Create);

        HighScoreData data = highScoreData;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static HighScoreData LoadHighScore()
    {
        string path = Application.persistentDataPath + "/player.kev";
        if (File.Exists(path))
        {
            Debug.Log(path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            HighScoreData data = formatter.Deserialize(stream) as HighScoreData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveCharacterData(CharacterData characterData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/kev.sucks";
        FileStream stream = new FileStream(path, FileMode.Create);
        CharacterData data = characterData;
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static CharacterData LoadCharacterData()
    {
        string path = Application.persistentDataPath + "/kev.sucks";
        if (File.Exists(path))
        {
            Debug.Log(path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            CharacterData data = formatter.Deserialize(stream) as CharacterData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
