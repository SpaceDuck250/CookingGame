using UnityEngine;

public class MenuDisplayScript : MonoBehaviour
{
    public MenuItemDisplayScript[] menuDisplays;
    public MealData[] menuItems = new MealData[7];

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

            if (menuItems[i] == null)
            {
                menuDisplays[i].Hide();
                continue;
            }

            menuDisplays[i].SetMenuItem(menuItems[i]);
        }
    }
}
