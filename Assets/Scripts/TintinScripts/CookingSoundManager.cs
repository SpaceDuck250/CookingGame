using UnityEngine;

public class CookingSoundManager : MonoBehaviour
{
    public AudioSource frierSrc;
    public AudioClip frierClip;

    public AudioSource steakSrc;
    public AudioClip steakClip;

    public AudioClip dispenseClip;

    public AudioClip successClip;
    public AudioClip failClip;

    public FrierInteractScript frierScript;
    public CookingInputOutputScript steakInputOutputScript;
    public CraftingTableScript vendingMachine;

    private void Start()
    {
        frierSrc.clip = frierClip;
        steakSrc.clip = steakClip;

        frierScript.OnFry += PlayFrierSound;
        frierScript.OnFryEnd += PauseFrierSound;

        steakInputOutputScript.OnCookingStart += PlaySteakSound;
        steakInputOutputScript.OnFoodTakenOutOfCookingStation += PauseSteakSound;

        frierScript.inputOutputScript.OnCookingSuccess += PlaySuccessSound;
        frierScript.inputOutputScript.OnCookingFail += PlayFailSound;

        steakInputOutputScript.OnCookingSuccess += PlaySuccessSound;
        steakInputOutputScript.OnCookingFail += PlayFailSound;

        frierSrc.Play();
        frierSrc.Pause();

        steakSrc.Play();
        steakSrc.Pause();

        vendingMachine.OnOuputDispensed += PlayDispenseSound;
    }

    private void OnDestroy()
    {
        frierScript.OnFry -= PlayFrierSound;
        frierScript.OnFryEnd -= PauseFrierSound;

        steakInputOutputScript.OnCookingStart -= PlaySteakSound;
        steakInputOutputScript.OnFoodTakenOutOfCookingStation -= PauseSteakSound;

        frierScript.inputOutputScript.OnCookingSuccess -= PlaySuccessSound;
        frierScript.inputOutputScript.OnCookingFail -= PlayFailSound;

        steakInputOutputScript.OnCookingSuccess -= PlaySuccessSound;
        steakInputOutputScript.OnCookingFail -= PlayFailSound;
    }

    public void PlayFrierSound(GameObject foodBeingFried)
    {
        frierSrc.UnPause();
    }

    public void PauseFrierSound()
    {
        frierSrc.Pause();
    }

    public void PlaySteakSound(FoodData foodData)
    {
        steakSrc.UnPause();
    }

    public void PauseSteakSound()
    {
        steakSrc.Pause();
    }

    public void PlaySuccessSound(Vector3 spawnPos, GameObject displayObj, Transform parent)
    {
        GeneralSoundManager.instance.PlaySoundEffect(successClip);
    }

    public void PlayFailSound(Vector3 spawnPos, GameObject displayObj, Transform parent)
    {
        GeneralSoundManager.instance.PlaySoundEffect(failClip);
    }

    public void PlayDispenseSound()
    {
        GeneralSoundManager.instance.PlaySoundEffect(dispenseClip);
    }
}