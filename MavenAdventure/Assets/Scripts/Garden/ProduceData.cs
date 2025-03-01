using UnityEngine;

[CreateAssetMenu(fileName = "New ProduceData", menuName = "Inventory/ProduceData")]
public class ProduceData : ScriptableObject
{
    public string displayName;
    public Sprite produceIcon;
    public int stackSize;
}