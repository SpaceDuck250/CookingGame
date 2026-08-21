using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemDisplayScript : MonoBehaviour
{
    public Image foodImage;
    public TMP_Text foodName;
    public TMP_Text foodPrice;

    public void SetMenuItem(Sprite image, string itemName, string itemPrice)
    {
        foodImage.sprite = image;
        foodName.text = itemName;
        foodPrice.text = itemPrice;
    }
}
