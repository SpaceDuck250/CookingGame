using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawnerScript : MonoBehaviour
{
    public GameObject customerPrefab;

    public Transform spawnPoint;
    public Transform stallTransform;
    public Transform exitTransform;

    public List<Transform> tableTransforms = new List<Transform>();
    public List<Transform> queuePositions = new List<Transform>();

    public float spawnInterval;
    public int maxCustomers;

    private float spawnTimer;
    private bool stallOccupied = false;
    private List<CustomerMovementScript> activeCustomers = new List<CustomerMovementScript>();
    private List<CustomerMovementScript> queue = new List<CustomerMovementScript>();

    private void Start()
    {
        spawnTimer = 0f;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnCustomer();
        }
    }

    private void TrySpawnCustomer()
    {
        if (activeCustomers.Count >= maxCustomers)
        {
            return;
        }

        if (queue.Count >= queuePositions.Count && stallOccupied)
        {
            return;
        }

        Transform freeTable = FindFreeTable();
        if (freeTable == null)
        {
            return;
        }

        SpawnCustomer(freeTable);
    }

    private void SpawnCustomer(Transform table)
    {
        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, customerPrefab.transform.rotation);

        CustomerMovementScript customerScript = newCustomer.GetComponent<CustomerMovementScript>();

        customerScript.stallTransform = stallTransform;
        customerScript.tableTransform = table;
        customerScript.exitTransform = exitTransform;

        customerScript.OnCustomerOrdered += OnCustomerOrdered;
        customerScript.OnCustomerServed += OnCustomerServed;
        customerScript.OnCustomerLeft += OnCustomerLeft;

        activeCustomers.Add(customerScript);

        if (!stallOccupied)
        {
            stallOccupied = true;
            customerScript.WalkToStall();
            return;
        }

        queue.Add(customerScript);
        customerScript.WalkToQueuePosition(queuePositions[queue.Count - 1]);
    }

    private void OnCustomerOrdered(CustomerMovementScript customer)
    {
        ShuffleQueue();
    }

    private void OnCustomerServed(CustomerMovementScript customer)
    {
        stallOccupied = false;

        if (queue.Count == 0)
        {
            return;
        }

        CustomerMovementScript nextCustomer = queue[0];
        queue.RemoveAt(0);

        stallOccupied = true;
        nextCustomer.WalkToStall();

        ShuffleQueue();
    }

    private void ShuffleQueue()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            queue[i].WalkToQueuePosition(queuePositions[i]);
        }
    }

    private Transform FindFreeTable()
    {
        List<Transform> occupiedTables = new List<Transform>();

        foreach (CustomerMovementScript customer in activeCustomers)
        {
            if (customer.tableTransform != null)
            {
                occupiedTables.Add(customer.tableTransform);
            }
        }

        foreach (Transform table in tableTransforms)
        {
            if (!occupiedTables.Contains(table))
            {
                return table;
            }
        }

        return null;
    }

    private void OnCustomerLeft(CustomerMovementScript customer)
    {
        customer.tableTransform = null;
        activeCustomers.Remove(customer);
    }
}