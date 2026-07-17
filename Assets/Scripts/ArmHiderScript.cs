using UnityEngine;

public class ArmHiderScript : MonoBehaviour
{
    public GameObject arms;

    private void Start()
    {
        PlayerHandScript.OnHoldSomething += ShowArms;
        PlayerHandScript.OnStopHoldSomething += HideArms;
    }

    private void OnDestroy()
    {
        PlayerHandScript.OnHoldSomething -= ShowArms;
        PlayerHandScript.OnStopHoldSomething -= HideArms;

    }

    public void ShowArms()
    {
        arms.SetActive(true);
    }

    public void HideArms()
    {
        arms.SetActive(false);

    }
}
