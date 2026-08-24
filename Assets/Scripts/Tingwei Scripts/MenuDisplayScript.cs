using UnityEngine;

public class MenuDisplayScript : MonoBehaviour
{
    [System.Serializable]
    public class MenuItem
    {
        public Sprite itemImage;
        public string itemName;
        public string itemprice;
    }

    public MenuItemDisplayScript[] menuDisplays;
    public MenuItem[] menuItems = new MenuItem[7];

    private void Start()
    {
        UpdateMenu();
    }

    public void UpdateMenu()
    {

        int amountToDisplay = Mathf.Min(menuDisplays.Length, menuItems.Length);



        for (int i = 0; i < amountToDisplay; i++)
        {
            if (menuDisplays[i] == null)
            {
                Debug.Log($"Menu Display {i} has not been assigned.");
                continue;
            }

            menuDisplays[i].SetMenuItem(menuItems[i].itemImage, menuItems[i].itemName, menuItems[i].itemprice);
        }
    }
}
