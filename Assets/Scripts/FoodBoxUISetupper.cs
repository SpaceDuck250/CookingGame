using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodBoxUISetupper : MonoBehaviour
{
    private FoodData foodStored;
    public BrownFoodBox brownFoodBox;

    public Image foodImage;
    public TextMeshProUGUI foodNameText;

    public TextMeshProUGUI amountText;

    private void Start()
    {
        foodStored = brownFoodBox.foodStored;
        SetupDesign();

        brownFoodBox.OnFoodAmountChangedInBox += UpdateUI;

        UpdateUI(brownFoodBox.foodStoredCount, brownFoodBox.maxFoodCount);
    }

    private void OnDestroy()
    {
        brownFoodBox.OnFoodAmountChangedInBox -= UpdateUI;

    }

    private void UpdateUI(int newAmount, int maxAmount)
    {
        amountText.text = newAmount.ToString() + "/" + maxAmount.ToString();
    }

    public void SetupDesign()
    {
        if (foodStored.foodSprite != null)
        {
            foodImage.sprite = foodStored.foodSprite;
        }

        foodNameText.text = foodStored.foodName;
    }
}
