using UnityEngine;
using System;

public class TrayRackScript : Interactable
{
    public int maxTrays = 8;

    public int trayLeft;

    public int trayStartAmount = 8;

    public GameObject[] trayDisplayList = new GameObject[8];

    public GameObject platterObj;
    public Transform spawnPoint;

    public event Action OnTrayTakenOut;
    public event Action OnTrayPlacedBack;

    private void Start()
    {
        trayLeft = trayStartAmount;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeldObj == null)
        {
            TakeTray(playerHand);
            return;
        }

        PlatterGiverScript giverScript = playerHand.currentFoodHeldObj.GetComponent<PlatterGiverScript>();
        if (playerHand.currentFoodHeldObj.tag == "Platter" && giverScript.platterScript.foodHeldList.Count == 0)
        {
            ReturnTray(playerHand);
            return;
        }
            
    }

    public void ReturnTray(PlayerHandScript playerHand)
    {
        if (trayLeft >= maxTrays)
        {
            return;
        }

        print("Returned");

        trayLeft++;

        SetTraysActive(trayLeft);

        playerHand.ClearFoodFromHand();

        OnTrayPlacedBack?.Invoke();
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

        OnTrayTakenOut?.Invoke();
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
