using UnityEngine;

public class FryCookerScript : MonoBehaviour
{
    public FrierInteractScript interactScript;
    public CookingInputOutputScript inputOutputScript;

    public bool frying = false;

    public GameObject fryObj;

    public float fryTime;
    public float burntTime;
    public float fryTimer;

    public float frySpeed;

    public bool fried = false;
    public bool burnt = false;

    public GameObject displayObj;

    private void Start()
    {
        interactScript.OnFry += OnFry;
        interactScript.OnFryEnd += OnFryEnd;

        interactScript.OnChangeHeatLevel += SetupFrySpeed;

        inputOutputScript.OnFoodTakenOutOfCookingStation += OnFryEnd;
    }

    private void OnDestroy()
    {
        interactScript.OnFry -= OnFry;
        interactScript.OnFryEnd -= OnFryEnd;

        interactScript.OnChangeHeatLevel -= SetupFrySpeed;

        inputOutputScript.OnFoodTakenOutOfCookingStation -= OnFryEnd;

    }

    private void Update()
    {
        if (!frying || burnt)
        {
            return;
        }

        DoFryTimer();
    }

    public void OnFry(GameObject objToFry)
    {
        if (objToFry != null)
        {
            frying = true;
        }

        // If the food hasnt changed
        if (fryObj == objToFry)
        {
            return;
        }

        fryObj = objToFry;

        SetupFryTimer();
    }

    public void OnFryEnd()
    {
        frying = false;
    }

    public void SetupFryTimer()
    {
        fryTimer = 0;
        fried = false;
        burnt = false;

        SetupFrySpeed(interactScript.currentHeatLevel);
    }

    public void SetupFrySpeed(FrierInteractScript.HeatLevel heatLevel)
    {
        frySpeed = heatLevel.speedIncreaser;
    }


    // Add overcooked later
    public void DoFryTimer()
    {
        fryTimer += Time.deltaTime * frySpeed;

        if (fryTimer >= fryTime && !fried)
        {
            fried = true;

            displayObj = CookingInputOutputScript.SpawnDisplayFoodInPosition(inputOutputScript.currentRecipeUsed.outputFood, interactScript.spawn, interactScript.spawnOffset, false);

            ICookStation cookStation = inputOutputScript.GetComponent<ICookStation>();
            cookStation.CallFoodSuccessEvent(interactScript.spawn.position, displayObj, interactScript.spawn);

            Destroy(fryObj);

            fryObj = null;
            interactScript.foodHeld = null;
        }
        else if (fryTimer >= burntTime && !burnt)
        {
            fryTimer = 0;

            burnt = true;

            frying = false;

            Destroy(displayObj);

            displayObj = CookingInputOutputScript.SpawnDisplayFoodInPosition(inputOutputScript.currentRecipeUsed.failedOutputFood, interactScript.spawn, interactScript.spawnOffset, false);

            ICookStation cookStation = inputOutputScript.GetComponent<ICookStation>();
            cookStation.CallFoodFailEvent(interactScript.spawn.position, displayObj, interactScript.spawn);

        }
    }
}
