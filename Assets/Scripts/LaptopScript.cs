using UnityEngine;

public class LaptopScript : Interactable
{
    public override void Interact(PlayerHandScript playerHand)
    {
        ShopScript.OnShopOpen?.Invoke();
    }
}
