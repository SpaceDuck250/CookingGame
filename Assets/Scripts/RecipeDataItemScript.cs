using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeDataItemScript : MonoBehaviour
{
    public Image outPutFood;
    public Image ingredientFood;

    public Image cookingStationImage;

    public TextMeshProUGUI recipeNameText;

    public GameObject plusSign;

    public void SetupRecipeItem(RecipeData recipe, Sprite cookingStationIcon)
    {
        CheckIfVendingRecipe(recipe, cookingStationIcon);

        cookingStationImage.sprite = cookingStationIcon;

        recipeNameText.text = recipe.recipeName;

        outPutFood.sprite = recipe.outputFood.foodSprite;

        CheckIfVendingRecipe(recipe, cookingStationIcon);
    }

    public void CheckIfVendingRecipe(RecipeData recipe, Sprite cookingStationIcon)
    {
        if (recipe.inputFood == null)
        {
            ingredientFood.sprite = cookingStationIcon;
            cookingStationImage.gameObject.SetActive(false);
            plusSign.SetActive(false);
        }
        else
        {
            ingredientFood.sprite = recipe.inputFood.foodSprite;

        }
    }

    
}
