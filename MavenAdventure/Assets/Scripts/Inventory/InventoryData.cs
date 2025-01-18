using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

[CreateAssetMenu(menuName = "Inventory System/ Inventory Item")]
public class InventoryData : ScriptableObject
{
    public int ID = -1;
    public string displayName;
    [TextArea(4, 4)]
    public string description;
    public Sprite icon;
    public int maxStackSize;
    public int GoldValue;
 }
