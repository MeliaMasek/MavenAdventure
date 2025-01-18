using UnityEngine;

[CreateAssetMenu(fileName = "SpriteSet", menuName = "Sets/SpriteSet")]
public class SpriteData : ScriptableObject
{
    public string itemName;
    public int price;

    public Sprite[] sprites;
}