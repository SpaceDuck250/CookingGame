using UnityEngine;

public class TrayRackScript : Interactable
{
    public int maxTrays = 8;

    // 1 to 8
    public int currentTrayIndex;

    public int trayLeft;

    public GameObject[] trayDisplayList = new GameObject[8];

    public GameObject platterObj;
    public Transform spawnPoint;

    private void Start()
    {
        currentTrayIndex = maxTrays;
        trayLeft = 8;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeldObj == null)
        {
            TakeTray(playerHand);
        }
        else
        {
            ReturnTray(playerHand);
        }
            
    }

    public void ReturnTray(PlayerHandScript playerHand)
    {
        if (trayLeft >= maxTrays)
        {
            return;
        }

        trayLeft++;

        SetTraysActive(trayLeft);

        playerHand.ClearFoodFromHand();
    }

    public void TakeTray(PlayerHandScript playerHand)
    {
        if (trayLeft <= 0)
        {
            return;
        }

        trayLeft--;

        SetTraysActive(trayLeft);

        Instantiate(platterObj, spawnPoint.position, Quaternion.identity);
    }

    public void SetTraysActive(int maxIndex)
    {
        foreach (GameObject tray in trayDisplayList)
        {
            tray.SetActive(false);
        }

        for (int i = 0; i < maxIndex; i++)
        {
            trayDisplayList[i].SetActive(true);
        }
    }

}
