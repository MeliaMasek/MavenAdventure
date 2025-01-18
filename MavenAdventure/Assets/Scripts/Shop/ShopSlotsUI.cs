using UnityEngine;
using UnityEngine.UI;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

public class ShopSlotsUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemCount;
    [SerializeField] private ShopSlots assignedItemSlot;
    
    public ShopSlots AssignedItemSlot => assignedItemSlot;

    [SerializeField] private Button addItemToCart;
    [SerializeField] private Button removeItemToCart;

    private int tempAmount;
    public ShopKeeperDisplay ParentDisplay { get; private set; }
    public float MarkUp { get; private set; }
    
    public int CharacterLimit { get; set; }

    private void Awake()
    {
        itemSprite.sprite = null;
        itemSprite.preserveAspect = true;
        itemSprite.color = Color.clear;
        itemName.text = "";
        itemCount.text = "";
        
        addItemToCart?.onClick.AddListener(AddItemToCart);
        removeItemToCart?.onClick.AddListener(RemoveItemFromCart);
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    public void Init(ShopSlots slot, float markUp)
    {
        assignedItemSlot = slot;
        MarkUp = markUp;
        tempAmount = slot.StackSize;
        UpdateUISLot();
    }
    
    private void UpdateUISLot()
    {
        if (assignedItemSlot.ItemData != null)
        {
            itemSprite.sprite = assignedItemSlot.ItemData.icon;
            itemSprite.color = Color.white;
            itemCount.text = assignedItemSlot.StackSize.ToString();
            var modifiedPrice = ShopKeeperDisplay.GetModifiedPrice(assignedItemSlot.ItemData, 1, MarkUp);
            itemName.text = $"{assignedItemSlot.ItemData.displayName} - {modifiedPrice}G";
        }
        else
        {
            itemSprite.sprite = null;
            itemSprite.color = Color.clear;
            itemName.text = "";
            itemCount.text = "";
        }
    }
    public void AddItemToCart()
    {
        if (tempAmount <= 0) return;
    
        tempAmount--;
        ParentDisplay.AddItemToCart(this, CharacterLimit); // Pass the character limit here
        itemCount.text = tempAmount.ToString();
    }
    
    public void RemoveItemFromCart()
    {
        if (tempAmount == assignedItemSlot.StackSize) return;
        {
            tempAmount++;
            ParentDisplay.RemoveItemFromCart(this);
            itemCount.text = tempAmount.ToString();
        }
    }
}
