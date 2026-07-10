using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderUIScript : MonoBehaviour
{
    public NpcDialogueScript dialogueScript;

    public GameObject OrderObj;
    public TextMeshProUGUI mealOrderName;
    public Image mealOrderSprite;

    private void Start()
    {
        NpcDialogueScript.OnTalkToCustomer += ShowOrder;
        NpcDialogueScript.OnEndTalkToCustomer += HideOrder;
        NpcDialogueScript.OnOrderMetTalk += FinishOrder;
    }

    private void OnDestroy()
    {
        NpcDialogueScript.OnTalkToCustomer -= ShowOrder;
        NpcDialogueScript.OnEndTalkToCustomer -= HideOrder;
        NpcDialogueScript.OnOrderMetTalk -= FinishOrder;
    }

    public void ShowOrder(CustomerData customerData, MealData mealOrdered)
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
