using UnityEngine;

public class FailChickenSpawner : MonoBehaviour
{
    public CookingInputOutputScript skewerScript;
    public GameObject rawChickenPrefab;

    private void Start()
    {
        skewerScript.OnCookingFail += OnCookingFail;
    }

    private void OnDestroy()
    {
        skewerScript.OnCookingFail -= OnCookingFail;

    }

    private void OnCookingFail(Vector3 arg1, GameObject arg2, Transform arg3)
    {
        Instantiate(rawChickenPrefab, transform.position, Quaternion.identity);
    }
}
