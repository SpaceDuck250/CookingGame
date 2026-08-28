using UnityEngine;

public class ShopSoundManager : MonoBehaviour
{
    public SFXBank shopBank;

    private void Start()
    {
        ShopScript.OnShopOpen += PlayOpenSound;
        ShopScript.OnShopClose += PlayCloseSound;
        ShopScript.OnSucessfullyBoughtFood += PlayPurchaseSound;
        ShopItemScript.OnSelectFoodItemInShop += PlaySelectSound;
    }

    private void OnDestroy()
    {
        ShopScript.OnShopOpen -= PlayOpenSound;
        ShopScript.OnShopClose -= PlayCloseSound;
        ShopScript.OnSucessfullyBoughtFood -= PlayPurchaseSound;
        ShopItemScript.OnSelectFoodItemInShop -= PlaySelectSound;
    }

    private void PlayOpenSound() => GeneralSoundManager.instance.PlaySoundEffect(shopBank, "open");
    private void PlayCloseSound() => GeneralSoundManager.instance.PlaySoundEffect(shopBank, "close");
    private void PlayPurchaseSound(FoodData food, int cost) => GeneralSoundManager.instance.PlaySoundEffect(shopBank, "purchase");
    private void PlaySelectSound(FoodData food) => GeneralSoundManager.instance.PlaySoundEffect(shopBank, "select");
}