using UnityEngine;

// for scenes without the save system like the tutorial
public class MoneySetupScript : MonoBehaviour
{
    public float startMoneyInThisScene;
    public MoneyManager moneyManager;

    private void Start()
    {
        moneyManager.SetMoney((decimal)startMoneyInThisScene);
    }
}
