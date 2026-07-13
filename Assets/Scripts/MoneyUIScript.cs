using UnityEngine;
using TMPro;

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

    private void OnMoneyChanged(float totalMoneyAmount, float earnedAmount, float tipAmount)
    {
        moneyUIText.text = ": " + totalMoneyAmount + "$";

        ShowEarnTextObj(earnedAmount, tipAmount);
    }

    private void ShowEarnTextObj(float amount, float tipAmount)
    {
        StopAllCoroutines();

        earnTextComponent.text = "+" + amount + "$";

        
        tipTextComponent.text = "+" + tipAmount + "$ Tip";

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

