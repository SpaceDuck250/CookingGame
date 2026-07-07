using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class CustomerSpawnerScript : MonoBehaviour
{
    //public GameObject customerPrefab;

    public List<GameObject> customerPrefabList = new List<GameObject>();

    public Transform spawnPoint;
    public Transform exitTransform;

    public Transform platterPoint;

    public List<Transform> chairTransforms = new List<Transform>();
    //public List<Transform> stallQueuePointList = new List<Transform>();
    public Transform[] stallQueuePointList = new Transform[4];

    public float spawnInterval;
    public int maxCustomers;
    private float spawnTimer;

    private List<CustomerMovementScript> activeCustomers = new List<CustomerMovementScript>();

    public static Action<CustomerMovementScript> OnCustomerLeftQueue;

    public static Action<CustomerMovementScript> OnCustomerSeated;
    public static Action<CustomerMovementScript> OnCustomerLeftSeat;

    public static Action<CustomerMovementScript> OnCustomerExit;

    public static CustomerSpawnerScript instance;

    public int emptyQueueIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spawnTimer = 0f;

        OnCustomerLeftQueue += OnCustomerOrderFinish;
        OnCustomerExit += OnCustomerDestroyed;
        OnCustomerSeated += CustomerSeated;
    }

    private void OnDestroy()
    {
        OnCustomerLeftQueue -= OnCustomerOrderFinish;
        OnCustomerExit -= OnCustomerDestroyed;
        OnCustomerSeated -= CustomerSeated;



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
        // Pick Random
        int randomInt = UnityEngine.Random.Range(0, customerPrefabList.Count);
        GameObject customerPrefab = customerPrefabList[randomInt];

        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, customerPrefab.transform.rotation);

        CustomerMovementScript customerScript = newCustomer.GetComponent<CustomerMovementScript>();

        customerScript.stallQueuePointTransform = queuePoint;
        customerScript.chairTransform = table;
        customerScript.exitTransform = exitTransform;

        activeCustomers.Add(customerScript);
    }

    private void ShuffleQueue()
    {
        if (emptyQueueIndex == stallQueuePointList.Length - 1)
        {
            return;
        }

        for (int i = emptyQueueIndex + 1; i < stallQueuePointList.Length; i++)
        {
            CustomerMovementScript customer = activeCustomers.FirstOrDefault(n => n.stallQueuePointTransform == stallQueuePointList[i]);
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
            if (customer.chairTransform != null)
            {
                occupiedTables.Add(customer.chairTransform);
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

    public int FindEmptyQueueIndex(Transform stallQueue)
    {
        for (int i = 0; i < stallQueuePointList.Length; i++)
        {
            if (stallQueuePointList[i] == stallQueue)
            {
                return i;
            }
        }

        return -1;
    }

    private void OnCustomerDestroyed(CustomerMovementScript customer)
    {
        //customer.tableTransform = null;

        activeCustomers.Remove(customer);
    }

    private void CustomerSeated(CustomerMovementScript customer)
    {
        customer.chairTransform = null;

    }

    private void OnCustomerOrderFinish(CustomerMovementScript customer)
    {
        emptyQueueIndex = FindEmptyQueueIndex(customer.stallQueuePointTransform);
        customer.stallQueuePointTransform = null;
        customer.orderDone = true;

        float waitTime = 2;
        Invoke("ShuffleQueue", waitTime);
    }
}