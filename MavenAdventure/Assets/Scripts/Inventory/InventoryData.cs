using System.Collections.Generic;
using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

[CreateAssetMenu(menuName = "Inventory System/ Inventory Item")]

public class InventoryData : ScriptableObject
{
    public int ID = -1;
    public string displayName;
    [TextArea(4, 4)]
    public string description;
    public Sprite Seedicon;
    public Sprite ProduceIcon;
    public int maxStackSize;
    public int GoldValue;
    public GameObject seedPrefab; // Only needed for planting
    public GameObject sproutPrefab; // Only needed for planting
    public GameObject maturePrefab; // Only needed for planting
    public int daysToSprout;  // Number of days to sprout
    public int daysToMature;  // Number of days to mature
    public List<string> tags = new List<string>();
}
