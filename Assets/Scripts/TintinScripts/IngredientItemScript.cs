using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientItemScript : MonoBehaviour
{
    public Image ingredientIcon;
    public TextMeshProUGUI ingredientNameText;

    public void SetupIngredientItem(FoodData food)
    {
        ingredientIcon.sprite = food.foodSprite;
        ingredientNameText.text = food.foodName;
    }
}