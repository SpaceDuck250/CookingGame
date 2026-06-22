using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SteakCookerScript : MonoBehaviour
{
    public SteakFlipperScript steakFlipper;

    public GameObject cookSide;

    public float timer;
    public float cookedTime;

    public float burntTime;

    public Material cookedMaterial;
    public Material burntMaterial;

    public bool canRunTimer = false;

    private void Start()
    {
        steakFlipper.OnSteakFlipped += OnSideSwitched;
    }

    private void OnDestroy()
    {
        steakFlipper.OnSteakFlipped -= OnSideSwitched;
    }

    private void Update()
    {
        CookSide();
    }

    private void CookSide()
    {
        if (!canRunTimer)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= cookedTime)
        {
            cookSide.GetComponent<SteakSideScript>().cooked = true;

            ApplyTexture(cookedMaterial);
        }

        if (timer >= burntTime)
        {
            cookSide.GetComponent<SteakSideScript>().burnt = true;

            ApplyTexture(burntMaterial);

            canRunTimer = false;
        }
    }

    private void OnSideSwitched(GameObject cookSide)
    {
        this.cookSide = cookSide;

        SteakSideScript steakSideScript = cookSide.GetComponent<SteakSideScript>();
        CheckIfAlreadyCookedOrBunt(steakSideScript);
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
        else if (side.cooked)
        {
            timer = cookedTime;
            canRunTimer = true;
        }
        else if (!side.burnt && !side.cooked)
        {
            timer = 0;
            canRunTimer = true;
        }

    }
}
