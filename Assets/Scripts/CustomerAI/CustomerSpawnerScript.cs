using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class CustomerSpawnerScript : MonoBehaviour
{
    public GameObject customerPrefab;

    public Transform spawnPoint;
    public Transform exitTransform;

    public List<Transform> chairTransforms = new List<Transform>();
    //public List<Transform> stallQueuePointList = new List<Transform>();
    public Transform[] stallQueuePointList = new Transform[4];

    public float spawnInterval;
    public int maxCustomers;
    private float spawnTimer;

    private List<CustomerMovementScript> activeCustomers = new List<CustomerMovementScript>();

    public static Action<CustomerMovementScript> OnCustomerLeftQueue;
    public static Action<CustomerMovementScript> OnCustomerLeftSeat;

    private void Start()
    {
        spawnTimer = 0f;

        OnCustomerLeftQueue += OnCustomerOrderFinish;
    }

    private void OnDestroy()
    {
        OnCustomerLeftQueue -= OnCustomerOrderFinish;

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

        Transform freeChair = FindFreeChair();
        Transform queuePoint = FindFreeQueuePoint();

        // Checks if any free chairs and if all queuePoints taken
        if (freeChair == null || queuePoint == null)
        {
            return;
        }

        SpawnCustomer(freeChair, queuePoint);
    }

    private void SpawnCustomer(Transform table, Transform queuePoint)
    {
        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, customerPrefab.transform.rotation);

        CustomerMovementScript customerScript = newCustomer.GetComponent<CustomerMovementScript>();

        customerScript.stallQueuePointTransform = queuePoint;
        customerScript.tableTransform = table;
        customerScript.exitTransform = exitTransform;

        activeCustomers.Add(customerScript);
    }

    private void ShuffleQueue()
    {
        for (int i = 0; i < stallQueuePointList.Length; i++)
        {
            CustomerMovementScript customer = activeCustomers.SingleOrDefault(n => n.stallQueuePointTransform == stallQueuePointList[i]);
            if (customer != null && i != 0)
            {
                customer.stallQueuePointTransform = stallQueuePointList[i - 1];
                customer.OnNewDestinationChange?.Invoke(customer.stallQueuePointTransform);
            }
        }
    }

    private Transform FindFreeQueuePoint()
    {
        List<Transform> takenQueuePoints = new List<Transform>();

        foreach (CustomerMovementScript customer in activeCustomers)
        {
            if (customer.stallQueuePointTransform != null)
            {
                takenQueuePoints.Add(customer.stallQueuePointTransform);
            }
        }

        for (int i = 0; i < stallQueuePointList.Length; i++)
        {
            if (!takenQueuePoints.Contains(stallQueuePointList[i]))
            {
                return stallQueuePointList[i];
            }
        }

        return null;
    }

    private Transform FindFreeChair()
    {
        List<Transform> occupiedTables = new List<Transform>();

        foreach (CustomerMovementScript customer in activeCustomers)
        {
            if (customer.tableTransform != null)
            {
                occupiedTables.Add(customer.tableTransform);
            }
        }

        foreach (Transform table in chairTransforms)
        {
            if (!occupiedTables.Contains(table))
            {
                return table;
            }
        }

        return null;
    }

    private void OnCustomerDestroyed(CustomerMovementScript customer)
    {
        customer.tableTransform = null;
    }

    private void OnCustomerSeated()
    {

    }

    private void OnCustomerOrderFinish(CustomerMovementScript customer)
    {
        customer.stallQueuePointTransform = null;

        float waitTime = 2;
        Invoke("ShuffleQueue", waitTime);
    }
}