using UnityEngine;

[CreateAssetMenu(fileName = "New ProduceData", menuName = "Inventory/ProduceData")]
public class ProduceData : ScriptableObject
{
    public int ID = -1;
    public string displayName;
    public Sprite produceIcon;
    public int maxStackSize;
    public int goldValue;
}