using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Customer;
using System.Collections.Generic;

public class OrderUIScript : MonoBehaviour
{
    public NpcDialogueScript dialogueScript;

    public GameObject OrderObj;
    public TextMeshProUGUI mealOrderName;
    public Image mealOrderSprite;

    public Transform foodsWantedParent;

    private void Start()
    {
        NpcDialogueScript.OnShowDialogue += ShowOrder;
        NpcDialogueScript.OnHideDialogue += HideOrder;
        NpcDialogueScript.OnOrderMetDialogue += FinishOrder;
    }

    private void OnDestroy()
    {
        NpcDialogueScript.OnShowDialogue -= ShowOrder;
        NpcDialogueScript.OnHideDialogue -= HideOrder;
        NpcDialogueScript.OnOrderMetDialogue -= FinishOrder;
    }

    public void ShowOrder(CustomerData customerData, MealData mealOrdered, CustomerMood mood)
    {
        //mealOrderName.text = mealOrdered.mealName;
        //if (mealOrdered.mealSprite != null)
        //{
        //    mealOrderSprite.sprite = mealOrdered.mealSprite;
        //}

        SetupAllFoodsWanted(mealOrdered);

        OrderObj.SetActive(true);
    }

    public void SetupAllFoodsWanted(MealData mealOrdered)
    {
        // Use absolute references later might be more performant

        List<Transform> foodIngredientObjList = new List<Transform>();

        foreach (Transform child in foodsWantedParent)
        {
            foodIngredientObjList.Add(child);

            child.gameObject.SetActive(false);
        }

        int currentFillIndex = 0;

        foreach (FoodData food in mealOrdered.foodIngredients)
        {
            SetupIngredientObj(food, foodIngredientObjList[currentFillIndex]);
            currentFillIndex++;
        }
    }

    public void SetupIngredientObj(FoodData food, Transform foodIngredientObj)
    {
        foodIngredientObj.GetComponent<Image>().sprite = food.foodSprite;
        foodIngredientObj.GetChild(0).GetComponent<TextMeshProUGUI>().text = food.foodName;

        foodIngredientObj.gameObject.SetActive(true);

    }

    public void HideOrder()
    {
        OrderObj.SetActive(false);

    }

    public void FinishOrder(CustomerData customer)
    {
        OrderObj.SetActive(false);

    }
}
