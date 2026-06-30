using System;
using UnityEngine;

public class SteakCookerScript : MonoBehaviour
{
    public SteakFlipperScript steakFlipper;

    public GameObject cookSide;
    public SteakSideScript steakSide;

    public float cookedTime;

    public float burntTime;

    public Material cookedMaterial;
    public Material burntMaterial;

    public bool canRunTimer = false;

    public CookingInputOutputScript cookingInputOutputScript;
    public bool steakFinished = false;

    public static event Action OnFoodBurnt;

    public Transform pickupObjParent;

    private void Start()
    {
        steakFlipper.OnSteakFlipped += OnSideSwitched;
        cookingInputOutputScript.OnCookingStart += OnCookingStart;
    }

    private void OnDestroy()
    {
        steakFlipper.OnSteakFlipped -= OnSideSwitched;
        cookingInputOutputScript.OnCookingStart -= OnCookingStart;
    }

    private void Update()
    {
        CookSide();
    }

    private void OnCookingStart(FoodData foodData)
    {
        Invoke("StartCookingBottom", 0.1f);
    }

    private void CookSide()
    {
        if (!canRunTimer)
        {
            return;
        }

        CheckIfSteakTakenOut();

        steakSide.cookedTimer += Time.deltaTime;

        if (steakSide.cookedTimer >= cookedTime )
        {
            steakSide.cooked = true;

            ApplyTexture(cookedMaterial);

            if (CheckIfBothSidesPerfectlyCooked(steakFlipper))
            {
                if (steakFlipper.steakHeld == null)
                {
                    return;
                }
                cookingInputOutputScript.OnCookingSuccess?.Invoke(steakFlipper.steakHeld.transform.position, steakFlipper.steakHeld, pickupObjParent);
            }
        }

        if (steakSide.cookedTimer >= burntTime)
        {
            steakSide.burnt = true;

            ApplyTexture(burntMaterial);

            //canRunTimer = false;
            EndCooking();

            if (steakFlipper.steakHeld == null)
            {
                return;
            }
            cookingInputOutputScript.OnCookingFail?.Invoke(steakFlipper.steakHeld.transform.position, steakFlipper.steakHeld, pickupObjParent);
        }
    }

    private void StartCookingBottom()
    {
        steakFinished = false;
        OnSideSwitched(steakFlipper.bottomPart);
    }

    private void EndCooking()
    {
        canRunTimer = false;
    }

    private void OnSideSwitched(GameObject cookSide)
    {
        this.cookSide = cookSide;

        steakSide = cookSide.GetComponent<SteakSideScript>();
        CheckIfAlreadyCookedOrBunt(steakSide);
    }

    private void ApplyTexture(Material material)
    {
        if (cookSide == null)
        {
            return;
        }
        Renderer sideRenderer = cookSide.GetComponent<Renderer>();
        sideRenderer.material = material;
    }

    public void CheckIfAlreadyCookedOrBunt(SteakSideScript side)
    {
        if (side.burnt)
        {
            canRunTimer = false;
        }
        else
        {
            canRunTimer = true;
        }
    }

    private bool CheckIfBothSidesPerfectlyCooked(SteakFlipperScript steakFlipper)
    {
        if (steakFinished || steakFlipper.steakHeld == null)
        {
            return false;
        }

        SteakSideScript bottomSide = steakFlipper.bottomPart.GetComponent<SteakSideScript>();
        SteakSideScript topSide = steakFlipper.topPart.GetComponent<SteakSideScript>();

        if (bottomSide.cooked && topSide.cooked)
        {
            steakFinished = true;
            return true;
        }

        return false;
    }

    private void CheckIfSteakTakenOut()
    {
        if (steakFlipper.steakHeld == null)
        {
            EndCooking();
        }
    }
}
