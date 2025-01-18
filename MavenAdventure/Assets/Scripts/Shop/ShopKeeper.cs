using UnityEngine;
using UnityEngine.Events;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList shopItemHeld;
    [SerializeField] private ShopSystem shopSystem;

    private ShopSaveData shopSaveData;
    
    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public string id;

    public AudioSource audioSource;
    public AudioClip[] audioClips;
    
    private void Awake()
    {
        shopSystem = new ShopSystem(shopItemHeld.Items.Count, shopItemHeld.MaxAllowedGold, shopItemHeld.BuyMarkUp, shopItemHeld.SellMarkUp);

        foreach (var item in shopItemHeld.Items)
        {
            shopSystem.AddToShop(item.ItemData, item.Amount);
        }
        
        id = GetComponent<UniqueID>().ID;
        shopSaveData = new ShopSaveData(shopSystem);
    }

    private void Start()
    {
        if (!SaveGameManager.data.shopKeeperDictionary.ContainsKey(id)) SaveGameManager.data.shopKeeperDictionary.Add(id, shopSaveData);
    }

    private void OnEnable()
    {
        SaveLoad.onLoadGame += LoadInv;
    }

    private void OnDisable()
    {
        SaveLoad.onLoadGame -= LoadInv;
    }

    private void LoadInv(SaveData data)
    {
        if (!data.shopKeeperDictionary.TryGetValue(id, out ShopSaveData _shopSaveData)) return;
        
        shopSaveData = _shopSaveData;
        shopSystem = shopSaveData.ShopSystem;
    }
    
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            if (audioClips != null && audioClips.Length > 0)
            {
                int randomIndex = Random.Range(0, audioClips.Length);
                AudioClip randomClip = audioClips[randomIndex];
                audioSource.clip = randomClip;
                audioSource.Play();
            }
            
            OnShopWindowRequested?.Invoke(shopSystem, playerInv);
            interactSuccessful = true;
        }
        else
        {
            interactSuccessful = false;
            Debug.Log("Player Inventory not found");
        }
    }

    public void EndInteraction()
    {
        
    }
}

[System.Serializable]
public class ShopSaveData
{
    public ShopSystem ShopSystem;
    
    public ShopSaveData(ShopSystem shopSystem)
    {
        ShopSystem = shopSystem;
    }
}
