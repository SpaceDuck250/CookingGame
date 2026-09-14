using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemDisplayScript : MonoBehaviour
{
    public Image foodImage;
    public TMP_Text foodName;
    public TMP_Text foodPrice;

    public void SetMenuItem(MealData meal)
    {
        gameObject.SetActive(true);

        foodImage.sprite = meal.mealSprite;
        foodName.text = meal.mealName;
        foodPrice.text = "$" + meal.mealPrice.ToString("0.00");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}