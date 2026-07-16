using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Customer;
using System.Collections;

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

    private List<CustomerStateMachine> activeCustomers = new List<CustomerStateMachine>();

    public static Action<CustomerStateMachine> OnCustomerLeftQueue;

    public static Action<CustomerStateMachine> OnCustomerSeated;
    public static Action<CustomerStateMachine> OnCustomerLeftSeat;

    public static Action<CustomerStateMachine> OnCustomerExit;

    public static CustomerSpawnerScript instance;

    public int emptyQueueIndex;

    // Special spawn, spawn companions with this customer
    public string specialCustomerName = "Uncle Fedrick";
    public int specialCompanionCount = 2;

    // Put the specific Fedrick-children prefabs here in the inspector.
    // When Uncle Fedrick spawns companions, they'll be picked from this list.
    public List<GameObject> fedrickChildrenPrefabs = new List<GameObject>();

    // Event Rush Hour, if true, will spawn more customers than usual
    public bool eventRushHour = false;

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

        SpawnCustomer(freeChair, queuePoint); // default: allow companions
    }

    // allowCompanions = false, prevents recursive companion-spawning when we spawn companions themselves
    // overridePrefab. if provided, spawn this prefab instead of picking randomly from customerPrefabList
    private void SpawnCustomer(Transform table, Transform queuePoint, bool allowCompanions = true, GameObject overridePrefab = null)
    {
        // Pick prefab (override if provided).
        GameObject prefabToInstantiate = overridePrefab;
        if (prefabToInstantiate == null)
        {
            if (customerPrefabList == null || customerPrefabList.Count == 0)
            {
                Debug.LogWarning("No customer prefab available to spawn.");
                return;
            }
            prefabToInstantiate = customerPrefabList[UnityEngine.Random.Range(0, customerPrefabList.Count)];
        }

        // small scatter so spawned objects don't stack exactly on top of each other
        Vector3 instPos = spawnPoint.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, UnityEngine.Random.Range(-0.5f, 0.5f));
        GameObject newCustomer = Instantiate(prefabToInstantiate, instPos, prefabToInstantiate.transform.rotation);

        //CustomerMovementScript customerScript = newCustomer.GetComponent<CustomerMovementScript>();
        CustomerStateMachine customerStateMachine = newCustomer.GetComponent<CustomerStateMachine>();

        customerStateMachine.counterPoint = queuePoint;
        customerStateMachine.seatPoint = table;
        customerStateMachine.exitPoint = exitTransform;
        customerStateMachine.trayReturnPoint = platterPoint;

        activeCustomers.Add(customerStateMachine);

        // If this is the special customer and companions are allowed, try to spawn companions
        if (allowCompanions && customerStateMachine.profile != null && customerStateMachine.profile.customerName == specialCustomerName && eventRushHour)
        {
            // Respect maxCustomers limit
            int availableSlots = maxCustomers - activeCustomers.Count;
            int spawnCount = Mathf.Min(specialCompanionCount, Mathf.Max(0, availableSlots));
            for (int i = 0; i < spawnCount; i++)
            {
                Transform freeChair = FindFreeChair();
                Transform freeQueue = FindFreeQueuePoint();

                if (freeChair == null || freeQueue == null)
                    break;

                // Choose a Fedrick child prefab
                GameObject childPrefab = null;
                if (fedrickChildrenPrefabs != null && fedrickChildrenPrefabs.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, fedrickChildrenPrefabs.Count);
                    childPrefab = fedrickChildrenPrefabs[index];
                }

                // Spawn companion without allowing them to spawn more companions
                SpawnCustomer(freeChair, freeQueue, false, childPrefab);
            }
        }

        // Start moving the customer(s) to the queue
        customerStateMachine.OnCustomerChangeState?.Invoke(CustomerState.WalkingToCounter);
    }

    private void ShuffleQueue()
    {
        if (emptyQueueIndex == stallQueuePointList.Length - 1)
        {
            return;
        }

        for (int i = emptyQueueIndex + 1; i < stallQueuePointList.Length; i++)
        {
            CustomerStateMachine customer = activeCustomers.FirstOrDefault(n => n.counterPoint == stallQueuePointList[i]);
            if (customer != null && i != 0)
            {
                customer.counterPoint = stallQueuePointList[i - 1];

                CustomerMovementScript movementScript = customer.GetComponent<CustomerMovementScript>();
                //movementScript.OnNewDestinationChange?.Invoke(customer.counterPoint);

                StartCoroutine(MoveToNextPoint(movementScript, customer.counterPoint));
            }
        }
    }

    private Transform FindFreeQueuePoint()
    {
        List<Transform> takenQueuePoints = new List<Transform>();

        foreach (CustomerStateMachine customer in activeCustomers)
        {
            if (customer.counterPoint != null)
            {
                takenQueuePoints.Add(customer.counterPoint);
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

        foreach (CustomerStateMachine customer in activeCustomers)
        {
            if (customer.seatPoint != null)
            {
                occupiedTables.Add(customer.seatPoint);
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

    private void OnCustomerDestroyed(CustomerStateMachine customer)
    {
        //customer.tableTransform = null;

        activeCustomers.Remove(customer);
    }

    private void CustomerSeated(CustomerStateMachine customer)
    {
        customer.seatPoint = null;
    }

    public void OnCustomerOrderFinish(CustomerStateMachine customer)
    {
        emptyQueueIndex = FindEmptyQueueIndex(customer.counterPoint);
        customer.counterPoint = null;
        customer.orderDone = true;

        //float waitTime = 2;
        //Invoke("ShuffleQueue", waitTime);

        ShuffleQueue();
    }

    public IEnumerator MoveToNextPoint(CustomerMovementScript move, Transform point)
    {
        yield return new WaitForSeconds(2);
        move.OnNewDestinationChange(point);
    }

    // Public accessor so other scripts can request a free queue point
    // Mostly for me to use but could be useful for other scripts too
    public Transform GetFreeQueuePoint()
    {
        return FindFreeQueuePoint();
    }

    public Transform GetFreeChair()
    {
        return FindFreeChair();
    }
}