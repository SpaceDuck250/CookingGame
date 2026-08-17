using UnityEngine;

public class TrayRackScript : Interactable
{
    public int maxTrays = 8;

    public int trayLeft;

    public GameObject[] trayDisplayList = new GameObject[8];

    public GameObject platterObj;
    public Transform spawnPoint;

    private void Start()
    {
        trayLeft = 8;
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
