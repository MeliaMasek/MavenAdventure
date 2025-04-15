using UnityEngine;
//Code created, borrowed, and modified from Chat GPT

[CreateAssetMenu(fileName = "New ProduceData", menuName = "Inventory/ProduceData")]
public class ProduceData : ScriptableObject
{
    public int ID = -1;
    public string displayName;
    public Sprite produceIcon;
    public int maxStackSize;
    public int goldValue;
}