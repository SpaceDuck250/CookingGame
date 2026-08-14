using UnityEngine;

public class MenuDisplayScript : MonoBehaviour
{
    public class MenuItem
    {
        public string itemName;
        public Sprite itemImage;
    }

    public MenuItemDisplayScript[] menuDisplays;
    public MenuItem[] menuItems = new MenuItem[5];

    private void Start()
    {
        UpdateMenu();
    }

    public void UpdateMenu()
    {
        int amountToDisplay = Mathf.Min(menuDisplays.Length, menuItems.Length);

        for (int i = 0; i < amountToDisplay; i++)
        {
            menuDisplays[i].SetMenuItem(menuItems[i].itemImage, menuItems[i].itemName);
        }
    }
}
