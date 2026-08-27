using UnityEngine;

public class CookingSoundManager : MonoBehaviour
{
    public SFXBank cookingBank;

    public AudioSource frierSrc;
    public AudioSource steakSrc;

    public CraftingTableScript vendingMachine;
    public FrierInteractScript frierScript;
    public CookingInputOutputScript steakInputOutputScript;

    private void Start()
    {
        frierSrc.clip = cookingBank.GetEntry("frier_sizzle").clip;
        steakSrc.clip = cookingBank.GetEntry("steak_sizzle").clip;

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

    public void PlayFrierSound(GameObject foodBeingFried) => frierSrc.UnPause();
    public void PauseFrierSound() => frierSrc.Pause();

    public void PlaySteakSound(FoodData foodData) => steakSrc.UnPause();
    public void PauseSteakSound() => steakSrc.Pause();

    public void PlaySuccessSound(Vector3 spawnPos, GameObject displayObj, Transform parent)
        => GeneralSoundManager.instance.PlaySoundEffect(cookingBank, "success", spawnPos);

    public void PlayFailSound(Vector3 spawnPos, GameObject displayObj, Transform parent)
        => GeneralSoundManager.instance.PlaySoundEffect(cookingBank, "fail", spawnPos);

    public void PlayDispenseSound() => GeneralSoundManager.instance.PlaySoundEffect(cookingBank, "dispense", transform.position);
    public void PlayButtonSound(SpecialRecipe newRecipe) => GeneralSoundManager.instance.PlaySoundEffect(cookingBank, "button", transform.position);
    public void PlayInsertSound() => GeneralSoundManager.instance.PlaySoundEffect(cookingBank, "insert", transform.position);
    public void PlayReturnSound() => GeneralSoundManager.instance.PlaySoundEffect(cookingBank, "return", transform.position);
}