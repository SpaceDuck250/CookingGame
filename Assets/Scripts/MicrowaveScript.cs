using UnityEngine;

public class MicrowaveScript : MonoBehaviour
{
    public CookingInputOutputScript inputOutputScript;

    public GameObject spinObject;

    public float timer;
    public float waitTime;

    public bool canRunTimer;

    public float rotateSpeed;

    public Transform spawnTransform;
    public Vector3 upOffset;

    private void Start()
    {
        inputOutputScript.OnCookingStart += OnCookingStart;
    }

    private void OnDestroy()
    {
        inputOutputScript.OnCookingStart -= OnCookingStart;

    }

    private void Update()
    {
        if (!canRunTimer)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            // Finish
        }

        RotateAround();
    }

    public void RotateAround()
    {
        spinObject.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

    }

    private void AddFoodModel(FoodData foodData)
    {
        GameObject newFoodModel = Instantiate(foodData.foodModel, spawnTransform.position, Quaternion.identity, spinObject.transform);

        newFoodModel.transform.localPosition = Vector3.zero;

        newFoodModel.transform.localPosition += upOffset;
    }

    private void OnCookingStart(FoodData foodData)
    {
        spinObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        AddFoodModel(foodData);

        canRunTimer = true;
    }
}
