using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyUIScript : MonoBehaviour
{
    public TextMeshProUGUI moneyUIText;

    public GameObject earnObj;
    public TextMeshProUGUI earnTextComponent;

    public GameObject tipObj;
    public TextMeshProUGUI tipTextComponent;

    private void Start()
    {
        MoneyManager.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnDestroy()
    {
        MoneyManager.OnMoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(decimal totalMoneyAmount, decimal earnedAmount, decimal tipAmount)
    {
        moneyUIText.text = ": " + totalMoneyAmount + "$";

        ShowEarnTextObj(earnedAmount, tipAmount);
    }

    private void ShowEarnTextObj(decimal amount, decimal tipAmount)
    {
        if (amount == 0)
        {
            return;
        }

        CancelInvoke("HideBothEarnObj");
        StopAllCoroutines();

        string sign = amount < 0 ? "-" : "+";
        Color color = amount < 0 ? Color.red : Color.green;

        amount = amount < 0 ? -amount : amount;

        earnTextComponent.text = sign + amount + "$";

        tipTextComponent.text = sign + tipAmount + "$ Tip";

        earnTextComponent.color = color;

        earnObj.SetActive(true);

        if (tipAmount > 0)
        {
            tipObj.SetActive(true);
        }

        float waitTime = 6f;
        Invoke("HideBothEarnObj", waitTime);
    }

    public void HideBothEarnObj()
    {
        earnObj.SetActive(false);
        tipObj.SetActive(false);
    }

}

