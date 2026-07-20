using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemShowerScript : MonoBehaviour
{
    public GameObject displayObj;
    public Image displayImage;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI displayCost;

    public FoodData currentSelectedFood;

    private void Start()
    {
        ShopItemScript.OnSelectFoodItemInShop += DisplayItem;
    }

    private void OnDestroy()
    {
        ShopItemScript.OnSelectFoodItemInShop -= DisplayItem;
    }

    public void DisplayItem(FoodData foodData)
    {
        displayObj.SetActive(true);
        currentSelectedFood = foodData;

        if (foodData.foodSprite != null)
        {
            displayImage.sprite = foodData.foodSprite;
        }

        displayName.text = foodData.foodName;
        displayCost.text = foodData.costInShop.ToString() + "$";


    }
}
