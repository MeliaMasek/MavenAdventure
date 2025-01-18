using UnityEngine;
using UnityEngine.UI;

//Code borrowed and Modified by Dan Pos from youtube https://www.youtube.com/watch?v=kSCY3b9kKsU

public class IngredientSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private Text itemCount;

    public InventoryData assignedData { get; private set; }
    
    public void Init(InventoryData data, int amount)
    {
        assignedData = data;
        itemSprite.preserveAspect = true;
        itemSprite.sprite = data.icon;
        itemSprite.color = Color.white;
        UpdateRequiredAmount(amount);
    }

    public void UpdateRequiredAmount(int amount, bool requiredItems = true)
    {
        itemCount.text = amount.ToString();
        if (!requiredItems) itemCount.text = Color.red.ToString();
        
    }
}
