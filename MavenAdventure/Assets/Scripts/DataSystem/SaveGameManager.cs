using System.Collections.Generic;
using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24
public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;
    
    private void Awake()
    {
        data = new SaveData();
        SaveLoad.onLoadGame += LoadData;
    }

    public static void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }
    
    public static void SaveData()
    {
        data.playerPosition = FindObjectOfType<CharacterMovement>().transform.position;

        data.savedClockTime = FindObjectOfType<CountdownTimer>().currentTime;
        
        var saveData = data;
        SaveLoad.Save(saveData);
    }   
    
    private static void LoadData(SaveData _data)
    {
        // Print out the saved player position
        Debug.Log("Saved player position: " + _data.playerPosition);

        // Load player position
        Vector3 savedPlayerPosition = _data.playerPosition;
        FindObjectOfType<CharacterMovement>().transform.position = savedPlayerPosition;

        // Print out the loaded player position
        Debug.Log("Loaded player position: " + savedPlayerPosition);

        FindObjectOfType<CountdownTimer>().currentTime = _data.savedClockTime;

        data = _data;
    }

    public static void TryToLoadData()
    {
        SaveLoad.Load();
        Debug.Log("Loaded Data");
    }
    
    private static List<InventoryData> GetCraftedItems()
    {
        CraftingBench craftingBench = FindObjectOfType<CraftingBench>();
        if (craftingBench != null)
        {
            return craftingBench.GetCraftedItems();
        }
        return new List<InventoryData>();
    }
}

