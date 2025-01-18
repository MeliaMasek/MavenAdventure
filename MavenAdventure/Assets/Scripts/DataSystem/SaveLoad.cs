using System.IO;
using UnityEngine;
using UnityEngine.Events;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24
public static class SaveLoad
{
    public static UnityAction OnSaveGame;
    public static UnityAction<SaveData> onLoadGame;

    private static string directory = "/SaveData/";
    private static string fileName = "SaveData.sav";

    public static bool Save(SaveData data)
    {
        OnSaveGame?.Invoke();

        string dir = Application.persistentDataPath + directory;

        //GUIUtility.systemCopyBuffer = dir; //Copy the path to the clipboard
        
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + fileName, json);

        Debug.Log("Saved Game");

        return true;
    }

    public static SaveData Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        SaveData data = new SaveData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);

            onLoadGame?.Invoke(data);
        }
        else
        {
            Debug.Log("No Save Data Found");
        }
        return data;
    }

    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}
