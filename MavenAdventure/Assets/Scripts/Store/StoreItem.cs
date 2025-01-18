using UnityEngine;

[CreateAssetMenu(fileName = "New Store Item", menuName = "Store/Store Item")]
public class StoreItem : ScriptableObject
{
    public string itemName;
    public string description;
    public int price;
    public Sprite icon;
    public bool isPurchased = false;
}