using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public IntData playerCoins; // Reference to the IntData ScriptableObject

    //public GameObject[] prefabSet1; // First set of prefabs
    //public Sprite[] spriteSet1; // Sprites corresponding to the first prefab set

    //public GameObject[] prefabSet2; // Second set of prefabs
    //public Sprite[] spriteSet2; // Sprites corresponding to the second prefab set

    //public GameObject[] prefabSet3;
    //public Sprite[] spriteSet3;
    
    //public GameObject[] prefabSet4;
    //public Sprite[] spriteSet4;

    public PrefabData prefabSet1;
    public SpriteData spriteSet1;

    public PrefabData prefabSet2;
    public SpriteData spriteSet2;
    
    public PrefabData prefabSet3;
    public SpriteData spriteSet3;

    public PrefabData prefabSet4;
    public SpriteData spriteSet4;
    
    public bool set1Purchased = false;
    public bool set2Purchased = false;
    public bool set3Purchased = false;
    public bool set4Purchased = false;

    void Start()
    {
        // Check if the sets have been purchased in a previous session and update the flags
        set1Purchased = PlayerPrefs.GetInt("Set1Purchased", 0) == 1;
        set2Purchased = PlayerPrefs.GetInt("Set2Purchased", 0) == 1;
        set3Purchased = PlayerPrefs.GetInt("Set3Purchased", 0) == 1;
        set4Purchased = PlayerPrefs.GetInt("Set4Purchased", 0) == 1;
    }
    
    public void PurchaseAndApplySet1(int cost)
    {   
        if (playerCoins.value >= cost && !set1Purchased)
        {
            playerCoins.value -= cost;
            set1Purchased = true; // Set the purchased flag for the item
            ApplyColorScheme(prefabSet1.prefabs, spriteSet1.sprites);
            Debug.Log("Item purchased and applied Set 1!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
        }
    }

    public void PurchaseAndApplySet2(int cost)
    {
        if (playerCoins.value >= cost && !set2Purchased)
        {
            playerCoins.value -= cost;
            set2Purchased = true; // Set the purchased flag for the item
            ApplyColorScheme(prefabSet2.prefabs, spriteSet2.sprites);
            Debug.Log("Item purchased and applied Set 2!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
        }
    }

    public void PurchaseAndApplySet3(int cost)
    {
        if (playerCoins.value >= cost && !set3Purchased)
        {
            playerCoins.value -= cost;
            set3Purchased = true; // Set the purchased flag for the item
            ApplyColorScheme(prefabSet3.prefabs, spriteSet3.sprites);
            Debug.Log("Item purchased and applied Set 3!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
        }
    }
    
    public void PurchaseAndApplySet4(int cost)
    {
        if (playerCoins.value >= cost && !set4Purchased)
        {
            playerCoins.value -= cost;
            set4Purchased = true; // Set the purchased flag for the item
            ApplyColorScheme(prefabSet4.prefabs, spriteSet4.sprites);
            Debug.Log("Item purchased and applied Set 4!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
        }
    }
    
    // Apply the selected prefab set's sprite set to the corresponding prefabs
    void ApplyColorScheme(GameObject[] prefabs, Sprite[] sprites)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            SpriteRenderer prefabRenderer = prefabs[i].GetComponent<SpriteRenderer>();

            if (prefabRenderer != null && i < sprites.Length)
            {
                prefabRenderer.sprite = sprites[i];
            }
            else
            {
                Debug.Log("Prefab " + i + " does not have a SpriteRenderer component or sprite is missing!");
            }
        }
    }
}