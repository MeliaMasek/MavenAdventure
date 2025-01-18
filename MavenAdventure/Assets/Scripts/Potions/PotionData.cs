using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Potion")]
public class PotionData : ScriptableObject 
{
        public string potionName;
        //public GameObject potionPrefab; 
        public Sprite potionIcon;
        public int potionValue;
}

