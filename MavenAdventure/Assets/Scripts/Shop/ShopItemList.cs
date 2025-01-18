using System.Collections.Generic;
using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

[CreateAssetMenu(menuName = "Shop/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> items;
    [SerializeField] private int maxAllowedGold;
    [SerializeField] private float sellMarkUp;
    [SerializeField] private float buyMarkUp;

    public List<ShopInventoryItem> Items => items;
    public int MaxAllowedGold => maxAllowedGold;
    public float SellMarkUp => sellMarkUp;
    public float BuyMarkUp => buyMarkUp;
}
[System.Serializable]
public struct ShopInventoryItem
{
    public InventoryData ItemData;
    public int Amount;
}