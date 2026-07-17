using UnityEngine;
using Customer;
using Category;

public class SnitchAIScript : MonoBehaviour
{
    public CustomerStateMachine customerStateMachine;

    public GameObject floatingTextPrefab;

    public Transform popupSpawnPoint;

    // Scoring
    public int startingScore = 100;
    public int reportThreshold = 50;

    // Deduction Options
    public int wrongOrderDeduction = 15;
    public int qualityDeduction = 20;
    public int longWaitDeduction = 25;

    private int currentScore;


    private bool hasDeductedForWait = false;
    private bool hasReported = false;

    private void Awake()
    {
        if (customerStateMachine == null)
        {
            customerStateMachine = GetComponent<CustomerStateMachine>();
        }

        currentScore = startingScore;
    }

    private void Start()
    {
        customerStateMachine.OnCustomerMoodChange += OnMoodChanged;

        if (customerStateMachine.mealChecker != null)
        {
            customerStateMachine.mealChecker.OnMealOrderFulfilled += OnServed;
            customerStateMachine.mealChecker.OnWrongOrderServed += OnWrongOrderGiven;
        }
    }

    private void OnDestroy()
    {
        customerStateMachine.OnCustomerMoodChange -= OnMoodChanged;

        if (customerStateMachine.mealChecker != null)
        {
            customerStateMachine.mealChecker.OnMealOrderFulfilled -= OnServed;
            customerStateMachine.mealChecker.OnWrongOrderServed -= OnWrongOrderGiven;
        }
    }

    private void OnMoodChanged(CustomerMood newMood)
    {
        if (newMood != CustomerMood.Angry || hasDeductedForWait)
        {
            return;
        }

        hasDeductedForWait = true;
        DeductPoints(longWaitDeduction, "customer got angry from waiting too long");
    }

    private void OnWrongOrderGiven()
    {
        bool servedBurntFood = customerStateMachine.mealChecker.CheckIfMealContainsCookType(CookAmount.Burnt);

        if (servedBurntFood)
        {
            DeductPoints(qualityDeduction, "poor quality (burnt) food served");
        }
        else
        {
            DeductPoints(wrongOrderDeduction, "wrong food served");
        }
    }

    private void DeductPoints(int amount, string reason)
    {
        currentScore -= amount;
        currentScore = Mathf.Max(currentScore, 0);

        Debug.Log("[Snitch] " + reason + ". -" + amount + " points (score now " + currentScore + ")");

        if (floatingTextPrefab != null)
        {
            SpawnFloatingText("-" + amount, Color.red);
        }
        else
        {
            Debug.LogWarning("[Snitch] floatingTextPrefab is not assigned on " + gameObject.name + " - no visual popup will show.");
        }
    }

    private void SpawnFloatingText(string text, Color color)
    {
        Vector3 spawnPosition = popupSpawnPoint != null ? popupSpawnPoint.position : transform.position + Vector3.up * 2f;

        GameObject popupInstance = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);

        SnitchFloatingTextScript floatingText = popupInstance.GetComponent<SnitchFloatingTextScript>();
        if (floatingText != null)
        {
            floatingText.SetText(text, color);
        }
    }

    private void OnServed()
    {
        if (hasReported)
        {
            return;
        }

        string customerName = customerStateMachine.profile != null ? customerStateMachine.profile.customerName : gameObject.name;

        Debug.Log("[Snitch] " + customerName + " was served. Final score: " + currentScore);

        if (currentScore < reportThreshold)
        {
            hasReported = true;
            HealthInspectorManager.ReportStall(customerName, currentScore);
        }
    }
}