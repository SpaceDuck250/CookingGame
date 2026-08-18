using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VendingMachineUIScript : MonoBehaviour
{
    public CraftingTableScript vendingMachine;

    public Image recipePicture;
    public TextMeshProUGUI recipeNameText;

    public Transform ingredientContainer;
    public GameObject ingredientItemTemplate;

    private void Start()
    {
        vendingMachine.OnCycleThroughRecipe += OnNewRecipeSelected;

        vendingMachine.CycleThroughRecipeList(1);
    }

    private void OnDestroy()
    {
        vendingMachine.OnCycleThroughRecipe -= OnNewRecipeSelected;

    }

    private void OnNewRecipeSelected(SpecialRecipe newRecipe)
    {
        recipePicture.sprite = newRecipe.recipeSprite;
        recipeNameText.text = newRecipe.recipeName;

        GenerateIngredientList(newRecipe);
    }

    private void GenerateIngredientList(SpecialRecipe recipe)
    {
        ClearIngredientContainer();

        foreach (FoodData ingredient in recipe.foodsNeededForRecipe)
        {
            GameObject newIngredientItem = Instantiate(ingredientItemTemplate, ingredientContainer);
            newIngredientItem.GetComponent<IngredientItemScript>().SetupIngredientItem(ingredient);
        }
    }

    private void ClearIngredientContainer()
    {
        foreach (Transform child in ingredientContainer)
        {
            Destroy(child.gameObject);
        }
    }
}