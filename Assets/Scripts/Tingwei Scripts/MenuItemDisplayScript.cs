using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemDisplayScript : MonoBehaviour
{
    public Image foodImage;
    public TMP_Text foodName;

    public void SetMenuItem(Sprite image, string itemName)
    {
        foodImage.sprite = image;
        foodName.text = itemName;
    }
}
