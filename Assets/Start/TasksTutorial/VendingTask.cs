using UnityEngine;

public class VendingTask : TutorialTask
{
    public CraftingTableScript vendingMachineScript;

    private void Start()
    {
        vendingMachineScript.OnOuputDispensed += OnOutputFood;
    }

    private void OnDestroy()
    {
        vendingMachineScript.OnOuputDispensed -= OnOutputFood;

    }

    public void OnOutputFood()
    {
        CompleteTask();
    }
}
