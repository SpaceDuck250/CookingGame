public class LaptopScript : Interactable
{
    public override void Interact(PlayerHandScript playerHand)
    {
        if (ShopScript.shopIsOpen) return;

        ShopScript.OnShopOpen?.Invoke();
    }
}