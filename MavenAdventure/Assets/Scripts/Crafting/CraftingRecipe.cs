using System.Collections.Generic;
using UnityEngine;

//Code borrowed and Modified by Dan Pos from youtube https://www.youtube.com/watch?v=kSCY3b9kKsU

[CreateAssetMenu(menuName = "Inventory System/ Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [SerializeField] private List<CraftingIngredient> ingredients;
    [SerializeField] private InventoryData craftedItem;
    [SerializeField, Min(1)] private int craftAmount = 1;
    
    public List<CraftingIngredient> Ingredients => ingredients;
    public InventoryData CraftedItem => craftedItem;
    public int CraftAmount => craftAmount;
}


[System.Serializable]
public struct CraftingIngredient
{
    public InventoryData ItemRequired;
    public int AmountRequired;
    
    public CraftingIngredient(InventoryData itemRequired, int amountRequired)
    {
        ItemRequired = itemRequired;
        AmountRequired = amountRequired;
    }
}