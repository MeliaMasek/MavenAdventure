using UnityEngine;
using UnityEngine.UI;

//Code borrowed and Modified by Dan Pos from youtube https://www.youtube.com/watch?v=kSCY3b9kKsU

public class CraftingListItemUI : MonoBehaviour
{
    [SerializeField] private Image recipeSprite;
    [SerializeField] private Text recipeName;
    [SerializeField] private Button craftButton;

    private CraftingDisplay parentDisplay;
    private CraftingRecipe recipe;

    private void Awake()
    {
        Debug.Log("CraftingListItemUI Awake");
        craftButton.onClick.AddListener(OnCraftButtonClicked);
    }

    public void Init(CraftingRecipe _recipe, CraftingDisplay _parentDisplay)
    {
       parentDisplay = _parentDisplay;
       recipe = _recipe;
       recipeSprite.sprite = recipe.CraftedItem.icon;
       recipeName.text = recipe.CraftedItem.displayName;
    }
    
    public void OnCraftButtonClicked()
    {
        Debug.Log("Craft button clicked for recipe: " + recipe.CraftedItem.displayName);

        if (parentDisplay == null) return;
        {
            parentDisplay.UpdateChosenRecipe(recipe);
        }
    }
}