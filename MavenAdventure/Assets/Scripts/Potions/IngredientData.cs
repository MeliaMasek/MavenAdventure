using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class IngredientData : ScriptableObject 
{
    public string ingredientName;
    public Sprite ingredientIcon;
}
