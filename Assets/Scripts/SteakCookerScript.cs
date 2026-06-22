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

        steakSide.cookedTimer += Time.deltaTime;

        if (steakSide.cookedTimer >= cookedTime)
        {
            steakSide.cooked = true;

            ApplyTexture(cookedMaterial);
        }

        if (steakSide.cookedTimer >= burntTime)
        {
            steakSide.burnt = true;

            ApplyTexture(burntMaterial);

            canRunTimer = false;
        }
    }

    private void StartCookingBottom()
    {
        OnSideSwitched(steakFlipper.bottomPart);
    }

    private void OnSideSwitched(GameObject cookSide)
    {
        this.cookSide = cookSide;

        steakSide = cookSide.GetComponent<SteakSideScript>();
        CheckIfAlreadyCookedOrBunt(steakSide);
    }

    private void ApplyTexture(Material material)
    {
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
}
