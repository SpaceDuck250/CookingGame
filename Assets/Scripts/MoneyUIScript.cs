using UnityEngine;
using TMPro;

public class MoneyUIScript : MonoBehaviour
{
    public TextMeshProUGUI moneyUIText;


    private void Start()
    {
        MoneyManager.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnDestroy()
    {
        MoneyManager.OnMoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(float moneyAmount)
    {
        moneyUIText.text = ": " + moneyAmount + "$";
    }
}

