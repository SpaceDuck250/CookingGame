using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class VendingMachineUIScript : MonoBehaviour
{
    public CraftingTableScript vendingMachine;

    public Image recipePicture;
    public TextMeshProUGUI recipeNameText;

    public Transform ingredientContainer;
    public GameObject ingredientItemTemplate;

    private List<IngredientItemScript> spawnedIngredientItems = new List<IngredientItemScript>();

    private void Start()
    {
        vendingMachine.OnCycleThroughRecipe += OnNewRecipeSelected;
        vendingMachine.OnFoodInputListChanged += RefreshHighlights;

        vendingMachine.CycleThroughRecipeList(1);
    }

    private void OnDestroy()
    {
        vendingMachine.OnCycleThroughRecipe -= OnNewRecipeSelected;
        vendingMachine.OnFoodInputListChanged -= RefreshHighlights;
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
            IngredientItemScript itemScript = newIngredientItem.GetComponent<IngredientItemScript>();
            itemScript.SetupIngredientItem(ingredient);

            spawnedIngredientItems.Add(itemScript);
        }
    }

    private void ClearIngredientContainer()
    {
        foreach (Transform child in ingredientContainer)
        {
            Destroy(child.gameObject);
        }

        spawnedIngredientItems.Clear();
    }

    private void RefreshHighlights()
    {
        foreach (IngredientItemScript item in spawnedIngredientItems)
        {
            bool isInserted = vendingMachine.foodInputList.Contains(item.assignedFood);
            item.SetHighlight(isInserted);
        }
    }
}