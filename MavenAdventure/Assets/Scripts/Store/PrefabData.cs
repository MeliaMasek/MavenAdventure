using UnityEngine;

[CreateAssetMenu(fileName = "PrefabSet", menuName = "Sets/PrefabSet")]
public class PrefabData : ScriptableObject
{
    public string itemName;
    public int price;

    public GameObject[] prefabs;
}
