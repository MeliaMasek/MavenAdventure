using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Code borrowed and Modified by Dan Pos from youtube https://www.youtube.com/watch?v=kSCY3b9kKsU

public class CraftingBench : MonoBehaviour, IInteractable
{
    [SerializeField] private List<CraftingRecipe> knownRecipe;

    private PlayerInventoryHolder playerInventory;
    
    private List<InventoryData> craftedItems = new List<InventoryData>(); // Define craftedItems here

    public List<CraftingRecipe> KnownRecipe => knownRecipe;
    public static UnityAction<CraftingBench> OnCraftingBenchRequested;
    
    #region Interctable Interface

        public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnCraftingBenchRequested?.Invoke(this);
        
        playerInventory = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInventory != null)
        {
            /*
            if (CheckIfCanCraft())
            {
                foreach (var ingredient in activeRecipe.Ingredients)
                {
                    playerInventory.PrimaryInventorySystem.RemoveItemFromInv(ingredient.ItemRequired, ingredient.AmountRequired);
                }
                playerInventory.PrimaryInventorySystem.AddToInventory(activeRecipe.CraftedItem, activeRecipe.CraftAmount);
            }   
            */
 
            EndInteraction();
            interactSuccessful = true;
        }
        else
        {
            interactSuccessful = false;
        }
    }

    /*
    private bool CheckIfCanCraft()
    {
        var itemsHeld = playerInventory.PrimaryInventorySystem.GetAllItemsHeld();

        foreach (var ingredient in activeRecipe.Ingredients)
        {
            if (!itemsHeld.TryGetValue(ingredient.ItemRequired, out int amountHeld)) return false;
            {
                if (amountHeld < ingredient.AmountRequired)
                {
                    return false;
                }
            }
        }
        return true;
    }
    */
    
    public void EndInteraction()
    {
        
    }
    #endregion
    
    public bool HasIngredients(CraftingRecipe recipe, int craftAmount)
    {
        var itemsHeld = playerInventory.PrimaryInventorySystem.GetAllItemsHeld();

        foreach (var ingredient in recipe.Ingredients)
        {
            if (!itemsHeld.TryGetValue(ingredient.ItemRequired, out int amountHeld) || amountHeld < ingredient.AmountRequired * craftAmount)
            {
                return false;
            }
        }
        return true;
    }

    public void ConsumeIngredients(CraftingRecipe recipe, int craftAmount)
    {
        foreach (var ingredient in recipe.Ingredients)
        {
            playerInventory.PrimaryInventorySystem.RemoveItemFromInv(ingredient.ItemRequired, ingredient.AmountRequired * craftAmount);
        }
    }

    public void AddCraftedItem(InventoryData craftedItem)
    {
        craftedItems.Add(craftedItem);
    }

    public void LoadCraftedItems(List<InventoryData> dataCraftedItems)
    {
        craftedItems = dataCraftedItems;
    }

    public List<InventoryData> GetCraftedItems()
    {
        return craftedItems;
    }
}
