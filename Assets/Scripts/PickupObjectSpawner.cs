using UnityEngine;

public class PickupObjectSpawner : MonoBehaviour
{
    public CookingInputOutputScript inputOutputScript;

    public GameObject pickupObject;

    private void Start()
    {
        inputOutputScript.OnCookingSuccess += SpawnPickupableOutputFood;
        inputOutputScript.OnCookingFail += SpawnFailObject;

    }

    private void OnDestroy()
    {
        inputOutputScript.OnCookingSuccess -= SpawnPickupableOutputFood;
        inputOutputScript.OnCookingFail -= SpawnFailObject;

    }

    private void SpawnPickupableOutputFood(Vector3 spawnPosition, GameObject deleteObject, Transform parent)
    {
        if (pickupObject != null)
        {
            Destroy(pickupObject);
        }

        pickupObject = inputOutputScript.SpawnPickupableOutputFood(spawnPosition, deleteObject, parent);
    }

    private void SpawnFailObject(Vector3 spawnPosition, GameObject deleteObject, Transform parent)
    {
        if (pickupObject != null)
        {
            Destroy(pickupObject);
        }

        pickupObject = inputOutputScript.SpawnPickupableOutputFood(spawnPosition, deleteObject, parent, false);
    }
}
