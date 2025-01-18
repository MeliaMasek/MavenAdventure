using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

[CreateAssetMenu(menuName = "Inventory System/ Item Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<InventoryData> itemDatabase;

    [ContextMenu("Set ID")]

    private void SetItemID()
    {
        itemDatabase = new List<InventoryData>();
        
        var foundItems = Resources.LoadAll<InventoryData>("ItemData").OrderBy(i => i.ID).ToList();

        var hasIDInRange = foundItems.Where(i =>i.ID != -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
        var hasAnIDNotInRange = foundItems.Where(i =>i.ID != -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
        var noID = foundItems.Where(i => i.ID <= -1).ToList();
        
        var index = 0;
        for (int i = 0; i < foundItems.Count; i++)
        {
            InventoryData itemToAdd;
            itemToAdd = hasIDInRange.Find(d => d.ID == i);

            if (itemToAdd != null)
            {
                itemDatabase.Add(itemToAdd);
            }
            else if (index < noID.Count)
            {
                noID[index].ID = i;
                itemToAdd = noID[index];
                index++;
                itemDatabase.Add(itemToAdd);
            }
        }

        foreach (var item in hasAnIDNotInRange)
        {
            itemDatabase.Add(item);
        }
    }   
    
    public InventoryData GetItem(int id)
    {
        return itemDatabase.Find(i => i.ID == id);
    }
}
