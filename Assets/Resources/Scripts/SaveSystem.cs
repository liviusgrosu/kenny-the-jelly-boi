using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveLevel(MenuSystem menu)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + menu.GetLevelName() + ".ld";
        FileStream stream = new FileStream(path, FileMode.Create); 

        LevelData data = new LevelData(menu);

        formatter.Serialize(stream, data);

        stream.Close();   
    }

    public static LevelData LoadLevel(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".ld";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();
            return data;
        }   
        else
        {
            return null;
        }     
    }
}