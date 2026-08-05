using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Customer;
using System.Collections;
using CustomerUtils;

public class CustomerSpawnerScript : MonoBehaviour
{
    //public GameObject customerPrefab;


    public List<GameObject> customerPrefabListToSpawn = new List<GameObject>();

    // For static utility use
    public List<CustomerNamePair> everyCustomerPrefabList = new List<CustomerNamePair>();
    public static Dictionary<string, GameObject> customerDictionary = new Dictionary<string, GameObject>();

    public Transform spawnPoint;
    public Transform exitTransform;

    public Transform platterPoint;

    // For rotation
    public Transform mainCounterPoint;

    public List<Transform> chairTransforms = new List<Transform>();
    //public List<Transform> stallQueuePointList = new List<Transform>();
    public Transform[] stallQueuePointList = new Transform[4];

    public float spawnInterval;
    public int maxCustomers;
    private float spawnTimer;

    private List<CustomerStateMachine> activeCustomers = new List<CustomerStateMachine>();
    // Get the number of active customers, removing any null references first
    // as in case a customer was destroyed without being removed from the list
    public int ActiveCustomerCount { get { activeCustomers.RemoveAll(customer => customer == null); return activeCustomers.Count; } }

    public static Action<CustomerStateMachine> OnCustomerLeftQueue;

    public static Action<CustomerStateMachine> OnCustomerSeated;
    public static Action<CustomerStateMachine> OnCustomerLeftSeat;

    public static Action<CustomerStateMachine> OnCustomerExit;

    // New event - fires after the spawner instantiates and registers a customer.
    // Parameters: spawned customer's state machine, the prefab that was instantiated.
    public event Action<CustomerStateMachine, GameObject> OnCustomerSpawned;
    public event Action<int> OnActiveCustomerCountChanged;

    public static CustomerSpawnerScript instance;

    public int emptyQueueIndex;

    // Event Rush Hour, if true, will spawn more customers than usual
    public bool eventRushHour = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AddEveryCustomerToDictionary();

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
        CustomerSpawnerInterval();
    }

    public void CustomerSpawnerInterval()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            // Rush Hour event spawns interval
            if (eventRushHour)
            {
                spawnInterval = UnityEngine.Random.Range(3f, 6f);
            }
            else if (activeCustomers.Count == 1)
            {
                spawnInterval = UnityEngine.Random.Range(2f, 4f);
            }
            else // Normal spawn interval
            {
                spawnInterval = UnityEngine.Random.Range(12f, 24f);
            }

            TrySpawnCustomer();
        }
    }

    public void TrySpawnCustomer()
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

        SpawnCustomer(freeChair, queuePoint); // default: allow companions handled externally
    }

    // allowCompanions = false, prevents recursive companion-spawning when we spawn companions themselves
    // overridePrefab. if provided, spawn this prefab instead of picking randomly from customerPrefabList
    // Made public so external handlers can spawn companions via the spawner API.
    public void SpawnCustomer(Transform table, Transform queuePoint, bool allowCompanions = true, GameObject overridePrefab = null)
    {
        // Pick prefab (override if provided).
        GameObject prefabToInstantiate = overridePrefab;
        if (prefabToInstantiate == null)
        {
            if (customerPrefabListToSpawn == null || customerPrefabListToSpawn.Count == 0)
            {
                Debug.LogWarning("No customer prefab available to spawn.");
                return;
            }
            prefabToInstantiate = customerPrefabListToSpawn[UnityEngine.Random.Range(0, customerPrefabListToSpawn.Count)];
        }

        // small scatter so spawned objects don't stack exactly on top of each other
        Vector3 instPos = spawnPoint.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, UnityEngine.Random.Range(-0.5f, 0.5f));
        GameObject newCustomer = Instantiate(prefabToInstantiate, instPos, prefabToInstantiate.transform.rotation);

        //CustomerMovementScript customerScript = newCustomer.GetComponent<CustomerMovementScript>();
        CustomerStateMachine customerStateMachine = newCustomer.GetComponent<CustomerStateMachine>();

        customerStateMachine.queuePoint = queuePoint;
        customerStateMachine.seatPoint = table;
        customerStateMachine.exitPoint = exitTransform;
        customerStateMachine.trayReturnPoint = platterPoint;

        activeCustomers.Add(customerStateMachine);

        // Notify subscribers that a customer was spawned (handler can spawn companions here).
        OnCustomerSpawned?.Invoke(customerStateMachine, prefabToInstantiate);

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
            CustomerStateMachine customer = activeCustomers.FirstOrDefault(n => n.queuePoint == stallQueuePointList[i]);
            if (customer != null && i != 0)
            {
                customer.queuePoint = stallQueuePointList[i - 1];

                CustomerMovementScript movementScript = customer.GetComponent<CustomerMovementScript>();
                //movementScript.OnNewDestinationChange?.Invoke(customer.counterPoint);

                StartCoroutine(MoveToNextPoint(movementScript, customer.queuePoint));
            }
        }
    }

    private Transform FindFreeQueuePoint()
    {
        List<Transform> takenQueuePoints = new List<Transform>();

        foreach (CustomerStateMachine customer in activeCustomers)
        {
            if (customer.queuePoint != null)
            {
                takenQueuePoints.Add(customer.queuePoint);
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
        emptyQueueIndex = FindEmptyQueueIndex(customer.queuePoint);
        customer.queuePoint = null;
        customer.orderDone = true;

        //float waitTime = 2;
        //Invoke("ShuffleQueue", waitTime);

        ShuffleQueue();
    }

    public IEnumerator MoveToNextPoint(CustomerMovementScript move, Transform point)
    {
        yield return new WaitForSeconds(2);
        if (move.customerStateMachine.currentMood != CustomerMood.ReallyAngry)
        {
            move.OnNewDestinationChange(point);
        }

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

    private void AddEveryCustomerToDictionary()
    {
        foreach (CustomerNamePair customer in everyCustomerPrefabList)
        {
            customerDictionary.Add(customer.customerName, customer.customerPrefab);
        }
    }
}

namespace CustomerUtils
{
    [Serializable]
    public class CustomerNamePair
    {
        public GameObject customerPrefab;

        // Should be unique
        public string customerName;
    }
}