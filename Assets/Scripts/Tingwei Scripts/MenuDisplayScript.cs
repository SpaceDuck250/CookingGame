using UnityEngine;

public class MenuDisplayScript : MonoBehaviour
{
    public MenuItemDisplayScript[] menuDisplays;
    public MealData[] menuItems;

    private void Start()
    {
        UpdateMenu();
    }

    public void UpdateMenu()
    {
        int amountToDisplay = Mathf.Min(menuDisplays.Length, menuItems.Length);

        for (int i = 0; i < menuDisplays.Length; i++)
        {
            if (i < amountToDisplay && menuItems[i] != null)
            {
                menuDisplays[i].SetMenuItem(menuItems[i]);
            }
            else
            {
                menuDisplays[i].Hide();
            }
        }
    }
}