
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
 
    public static void Save(SaveManager saveManager) 
    {
        BinaryFormatter formatter = new BinaryFormatter();
       string path = Application.persistentDataPath + "/save.knight";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(saveManager);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static PlayerData Load()
    {
        string path = Application.persistentDataPath + "/save.knight";
        if (File.Exists(path))
        {
            Debug.Log("Save file found in" + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in" + path);
            return null;
        }
    }
}