using UnityEngine;

public class SpawnCustomer : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    private GameObject currentCustomer;

    void Update()
    {
        // Already have a customer spawned
        if (currentCustomer != null)
            return;

        Summon();
    }

    void Summon()
    {
        if (customerPrefabs.Length == 0)
            return;

        int index = Random.Range(0, customerPrefabs.Length);

        currentCustomer = Instantiate(
            customerPrefabs[index],
            transform.position,
            transform.rotation);
    }

    public void CustomerLeft()
    {
        currentCustomer = null;
    }
}