using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeDataItemScript : MonoBehaviour
{
    public Image outPutFood;
    public Image ingredientFood;

    public Image cookingStationImage;

    public TextMeshProUGUI recipeNameText;

    public void SetupRecipeItem(RecipeData recipe, Sprite cookingStationIcon)
    {
        outPutFood.sprite = recipe.outputFood.foodSprite;

        ingredientFood.sprite = recipe.inputFood.foodSprite;

        cookingStationImage.sprite = cookingStationIcon;

        recipeNameText.text = recipe.recipeName;
    }
}
