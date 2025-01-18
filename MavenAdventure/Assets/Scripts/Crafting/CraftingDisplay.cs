using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Code borrowed and Modified by Dan Pos from youtube https://www.youtube.com/watch?v=kSCY3b9kKsU

public class CraftingDisplay : MonoBehaviour
{
    [Header("Recipe List Window")]
    [SerializeField] private GameObject recipeListPanel;
    [SerializeField] private CraftingListItemUI craftingListItemUI; 
    
    [Header("Ingredient Window")]
    [SerializeField] private IngredientSlotUI ingredientSlotPrefab;
    [SerializeField] private Transform ingredientGrid;
    [SerializeField] private Button increaseCraftAmountButton;
    [SerializeField] private Button decreaseCraftAmountButton;
    [SerializeField] private Text craftAmountText;
    [SerializeField] private Button craftButton;
    
    [Header("Item Preview Section")]
    [SerializeField] private Image itemPreviewSprite;
    [SerializeField] private Text itemPreviewName;
    [SerializeField] private Text itemPreviewDescription;
    
    private int craftAmount = 1;
    private List<IngredientSlotUI> ingredientSlotsUI = new List<IngredientSlotUI>();

    [SerializeField] private float addedTime;
    [SerializeField] private Text addedTimeText;

    private CraftingBench craftingBench; 
    private CraftingRecipe chosenRecipe;
    [SerializeField] public PlayerInventoryHolder playerInventoryHolder;
    
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private void Awake()
   {
       increaseCraftAmountButton.onClick.RemoveAllListeners();
       decreaseCraftAmountButton.onClick.RemoveAllListeners();
       
       increaseCraftAmountButton.gameObject.SetActive(false);
       decreaseCraftAmountButton.gameObject.SetActive(false);

       craftButton.onClick.AddListener(CraftItem);

       craftAmountText.text = "";
   }

   private void ChangeCraftAmount(int amount)
   {
       if (craftAmount <= 1 && amount < 0) return;

       craftAmount += amount;
       craftAmountText.text = craftAmount.ToString();
       
       RefreshRecipeGrid();
       
       if (craftAmount <= 1) craftAmount = 1;
   }

   public void DisplayCraftingWindow(CraftingBench _craftingBench)
   {
       craftingBench = _craftingBench;
       ClearItemPreview();
       RefreshListDisplay();
   }

   private void RefreshListDisplay()
   {
       ClearSlots(recipeListPanel.transform);

       foreach (var recipe in craftingBench.KnownRecipe)
       {
            var recipeSlots = Instantiate(craftingListItemUI, recipeListPanel.transform);
            recipeSlots.Init(recipe, this );
       }
   }
   private void ClearSlots(Transform transformToDestroy)
   {
       foreach (var item in transformToDestroy.transform.Cast<Transform>())
       {
           Destroy(item.gameObject);
       }
   }
   
   private void ClearItemPreview()
   {
       itemPreviewSprite.sprite = null;
       itemPreviewSprite.color = Color.clear;
       itemPreviewName.text = "";
       itemPreviewDescription.text = "";
   }
   
   private void DisplayItemPreview(InventoryData data)
   {
       itemPreviewSprite.sprite = data.icon;
       itemPreviewSprite.color = Color.white;
       itemPreviewName.text = data.displayName;
       itemPreviewDescription.text = data.description;
   }
   
   public void UpdateChosenRecipe(CraftingRecipe recipe)
   {
       chosenRecipe = recipe;
       DisplayItemPreview(chosenRecipe.CraftedItem);
       RefreshRecipeWindow();
   }

   private void RefreshRecipeWindow()
   {
       ClearSlots(ingredientGrid);

       ingredientSlotsUI.Clear();

       craftAmount = 1;
       craftAmountText.text = craftAmount.ToString();

       craftAmountText.gameObject.SetActive(true);

       increaseCraftAmountButton.gameObject.SetActive(true);
       decreaseCraftAmountButton.gameObject.SetActive(true);

       increaseCraftAmountButton.onClick.RemoveAllListeners();
       decreaseCraftAmountButton.onClick.RemoveAllListeners();

       increaseCraftAmountButton.onClick.AddListener(() => ChangeCraftAmount(1));
       decreaseCraftAmountButton.onClick.AddListener(() => ChangeCraftAmount(-1));

       craftButton.interactable = true;

       foreach (var ingredient in chosenRecipe.Ingredients)
       {
           var ingredientSlot = Instantiate(ingredientSlotPrefab, ingredientGrid);
           ingredientSlot.Init(ingredient.ItemRequired, ingredient.AmountRequired);
           ingredientSlotsUI.Add(ingredientSlot);
       }
       
       bool enoughIngredients = craftingBench.HasIngredients(chosenRecipe, craftAmount);

       craftButton.interactable = enoughIngredients;
   }

   private void RefreshRecipeGrid()
   {
       foreach (var ingredient in chosenRecipe.Ingredients)
       {
           foreach (var slot in ingredientSlotsUI)
           {
               if (slot.assignedData == ingredient.ItemRequired)
               {
                   slot.UpdateRequiredAmount(ingredient.AmountRequired * craftAmount);
               }
           }
       }
   }
   
   private void CraftItem()
   {
       if (chosenRecipe == null)
       {
           Debug.LogWarning("No recipe chosen to craft.");
           return;
       }

       if (!craftingBench.HasIngredients(chosenRecipe, craftAmount))
       {
           Debug.LogWarning("Not enough ingredients to craft.");
           return;
       }

       craftingBench.ConsumeIngredients(chosenRecipe, craftAmount);

       if (playerInventoryHolder.AddItemToInventory(chosenRecipe.CraftedItem, craftAmount))
       {
           Debug.Log("Crafted item added to inventory successfully.");
           craftingBench.AddCraftedItem(chosenRecipe.CraftedItem);
           
           if (audioClips != null && audioClips.Length > 0)
           {
               int randomIndex = Random.Range(0, audioClips.Length);
               AudioClip randomClip = audioClips[randomIndex];
               audioSource.clip = randomClip;
               audioSource.Play();
           }

           CountdownTimer countdownTimer = FindObjectOfType<CountdownTimer>();
           if (countdownTimer != null)
           {
               countdownTimer.countdownTime += addedTime;
               countdownTimer.currentTime += addedTime;
               Debug.Log("Added additional time to countdown. New currentTime: " + countdownTimer.currentTime);
           }
           else
           {
               Debug.LogWarning("Countdown timer not found.");
           }
           
           if (addedTimeText != null)
           {
               addedTimeText.text = "+" + addedTime.ToString() + " sec";
               StartCoroutine(AddedTimeText());
           }
           else
           {
               Debug.LogWarning("Added time text reference is missing.");
           }
       }
       else
       {
           Debug.LogWarning("Failed to add crafted item to inventory.");
           return;
       }

       increaseCraftAmountButton.gameObject.SetActive(false);
       decreaseCraftAmountButton.gameObject.SetActive(false);
       craftAmountText.gameObject.SetActive(false);
       craftButton.interactable = false;

       RefreshListDisplay();
       ClearItemPreview();
       ClearSlots(ingredientGrid);
   }

   private IEnumerator AddedTimeText()
   {
       if (addedTimeText != null)
       {
           addedTimeText.gameObject.SetActive(true);
           yield return new WaitForSeconds(.75f); // Adjust the duration as needed
           addedTimeText.gameObject.SetActive(false);
       }
   }
   
   public void CloseCraftingWindow()
   {
       gameObject.SetActive(false);
       ResetCraftingUI();
   }
   
   public void ResetCraftingUI()
   {
       ClearItemPreview();
       ClearSlots(ingredientGrid);
   }
}
