using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VendingMachineUIScript : MonoBehaviour
{
    public CraftingTableScript vendingMachine;

    public Image recipePicture;
    public TextMeshProUGUI recipeNameText;

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
    }
}
