using UnityEngine;

public class ArmBugFixer : MonoBehaviour
{
    public GameObject arm;
    public GameObject food;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<HoldableFoodScript>() != null)
        {
            return;
        }

        arm.SetActive(false);
        food.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        arm.SetActive(true);
        food.SetActive(true);
    }
}
