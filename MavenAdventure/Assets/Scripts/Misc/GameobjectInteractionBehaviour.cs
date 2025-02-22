using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//code borrowed and modified from Stackoverflow https://gamedev.stackexchange.com/questions/180033/best-way-make-static-gameobject-clickable-2d#:~:text=1%20Answer&text=If%20you%20are%20trying%20to,is%20the%20way%20to%20go.&text=Use%20the%20LayerMask%20parameter%20of,You%20can%20then%20call%20hit.
public class GameobjectInteractionBehaviour : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject CraftingMenu;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Clicked " + name);
            Crafting();
        }
    }

    public void Crafting()
    {
        CraftingMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    /*
    public void CraftPotion()
    {
        // Assuming you have a reference to the PotionCrafting script
        if (playerInventory != null)
        {
            // Get the selected ingredients from the player's inventory
            List<Ingredient> selectedIngredients = playerInventory.GetSelectedIngredients();

            // Call the crafting method from PotionCrafting script to combine ingredients
            Potion craftedPotion = PotionCrafting.Instance.CombineIngredients(selectedIngredients);

            if (craftedPotion != null)
            {
                // Success: Display the crafted potion to the player
                Debug.Log("Crafted potion: " + craftedPotion.potionName);
                // Optionally, you can do something with the crafted potion, such as adding it to the player's inventory or applying its effects
            }
            else
            {
                // Error: Display a message to the player indicating no valid combination found
                Debug.Log("Invalid combination!");
            }
        }
    }
    */
}