using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Customer;

public class OrderUIScript : MonoBehaviour
{
    public NpcDialogueScript dialogueScript;

    public GameObject OrderObj;
    public TextMeshProUGUI mealOrderName;
    public Image mealOrderSprite;

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
        mealOrderName.text = mealOrdered.mealName;
        if (mealOrdered.mealSprite != null)
        {
            mealOrderSprite.sprite = mealOrdered.mealSprite;
        }

        OrderObj.SetActive(true);
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
