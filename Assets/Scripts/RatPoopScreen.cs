using UnityEngine;

public class RatPoopScreen : MonoBehaviour
{
    public GameObject ratPoopScreen;

    private void Start()
    {
        RatPoopScript.OnStepOnRatPoop += OnStepOnRatPoop;
    }

    private void OnDestroy()
    {
        RatPoopScript.OnStepOnRatPoop -= OnStepOnRatPoop;

    }

    private void OnStepOnRatPoop()
    {
        StopAllCoroutines();
        ratPoopScreen.SetActive(true);
        Invoke("HideRatPoopScreen", 2f);
    }

    private void HideRatPoopScreen()
    {
        ratPoopScreen.SetActive(false);

    }
}
