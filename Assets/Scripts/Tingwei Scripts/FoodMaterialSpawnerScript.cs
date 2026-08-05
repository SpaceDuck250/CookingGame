using UnityEngine;

public class FoodMaterialSpawnerScript : MonoBehaviour
{
    public GameObject spawnItem;
    public Transform spawnPoint;

    private void OnMouseDown()
    {
        SpawnFoodItem();
    }

    private void SpawnFoodItem()
    {
        if (spawnItem == null)
        {
            Debug.Log("No food item has been assigned.");
            return;
        }

        if (spawnPoint != null)
        {
            Instantiate(spawnItem, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Instantiate(spawnItem, transform.position, transform.rotation);
        }
    }
}
