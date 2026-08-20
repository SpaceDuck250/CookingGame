using UnityEngine;

public class CookingSoundManager : MonoBehaviour
{
    public AudioSource frierSrc;
    public AudioClip frierClip;

    public AudioSource steakSrc;
    public AudioClip steakClip;

    public AudioClip successClip;
    public AudioClip failClip;

    public AudioClip dispenseClip;
    public AudioClip buttonClip;
    public AudioClip insertClip;
    public AudioClip returnClip;
    public CraftingTableScript vendingMachine;

    public FrierInteractScript frierScript;
    public CookingInputOutputScript steakInputOutputScript;

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

        vendingMachine.OnOuputDispensed += PlayDispenseSound;
        vendingMachine.OnCycleThroughRecipe += PlayButtonSound;
        vendingMachine.OnFoodInputListChanged += PlayInsertSound;
        vendingMachine.OnFoodReturned += PlayReturnSound;

        frierSrc.Play();
        frierSrc.Pause();

        steakSrc.Play();
        steakSrc.Pause();
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

        vendingMachine.OnOuputDispensed -= PlayDispenseSound;
        vendingMachine.OnCycleThroughRecipe -= PlayButtonSound;
        vendingMachine.OnFoodInputListChanged -= PlayInsertSound;
        vendingMachine.OnFoodReturned -= PlayReturnSound;
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

    public void PlayButtonSound(SpecialRecipe newRecipe)
    {
        GeneralSoundManager.instance.PlaySoundEffect(buttonClip);
    }

    public void PlayInsertSound()
    {
        GeneralSoundManager.instance.PlaySoundEffect(insertClip);
    }

    public void PlayReturnSound()
    {
        GeneralSoundManager.instance.PlaySoundEffect(returnClip);
    }
}