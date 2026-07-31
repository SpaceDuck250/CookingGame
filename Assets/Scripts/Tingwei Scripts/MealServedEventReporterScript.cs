using UnityEngine;

[RequireComponent(typeof(MealChecker))]
public class MealServedEventReporterScript : MonoBehaviour
{
    public MealChecker mealChecker;
    public AIEventDataScript eventData;

    private void Awake()
    {
        mealChecker = GetComponent<MealChecker>();
    }

    private void Start()
    {
        FindEventData();
    }

    private void OnEnable()
    {
        if (mealChecker == null)
        {
            mealChecker = GetComponent<MealChecker>();
        }

        if (mealChecker != null)
        {
            mealChecker.OnMealOrderFulfilled += HandleMealOrderFulfilled;
        }
    }

    private void OnDisable()
    {
        if (mealChecker != null)
        {
            mealChecker.OnMealOrderFulfilled -= HandleMealOrderFulfilled;
        }
    }

    // Call when meal order fulfilled
    private void HandleMealOrderFulfilled()
    {
        if (eventData == null)
        {
            FindEventData();
        }

        if (eventData == null)
        {
            Debug.Log("MealServedEventReporter could not find AIEventDataScript.");
            return;
        }

        // Record the dish served in the event data
        eventData.RecordDishServed();

        //Debug.Log("Dish recorded. Recent dishes served: " + eventData.DishesServedAtOnce);
    }

    private void FindEventData()
    {
        if (AIEventSystemScript.Instance != null)
        {
            eventData = AIEventSystemScript.Instance.eventData;
        }
    }
}
