using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientItemScript : MonoBehaviour
{
    public Image ingredientIcon;
    public TextMeshProUGUI ingredientNameText;

    public GameObject highlightOutline;

    [HideInInspector] public FoodData assignedFood;

    public void SetupIngredientItem(FoodData food)
    {
        assignedFood = food;

        ingredientIcon.sprite = food.foodSprite;
        ingredientNameText.text = food.foodName;

        SetHighlight(false);
    }

    public void SetHighlight(bool isInserted)
    {
        highlightOutline.SetActive(isInserted);
    }
}