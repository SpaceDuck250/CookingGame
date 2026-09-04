public class LaptopScript : Interactable
{
    private bool canOpen = true;

    private void Start()
    {
        DaySystemManager.OnDayStart += OnDayStart;
        DaySystemManager.OnDayEnd += OnDayEnd;
    }

    private void OnDestroy()
    {
        DaySystemManager.OnDayStart -= OnDayStart;
        DaySystemManager.OnDayEnd -= OnDayEnd;
    }

    public void OnDayStart()
    {
        canOpen = true;
    }

    public void OnDayEnd(PlayerDailyStats playerDailyStats)
    {
        canOpen = false;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (!canOpen)
        {
            return;
        }

        if (ShopScript.shopIsOpen) return;

        ShopScript.OnShopOpen?.Invoke();
    }
}