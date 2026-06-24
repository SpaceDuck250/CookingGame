using UnityEngine;
using System;

public class SteakFlipperScript : Interactable
{
    public GameObject flipObject;
    public GameObject steakHeld;

    public float rotateAmount = 90;

    public float rotateSpeed;

    Quaternion desiredRotation = Quaternion.identity;

    public event Action<GameObject> OnSteakFlipped;
    public GameObject topPart;
    public GameObject bottomPart;

    public GameObject sideGettingCooked;

    //For setup
    public CookingInputOutputScript cookingInputOutput;
    public GameObject rawSteakPrefab;
    public Vector3 localPositionOffset;

    public InteractAreaScript interactScript;

    private void Start()
    {
        desiredRotation = Quaternion.Euler(0, 0, 90);

        cookingInputOutput.OnCookingStart += OnCookingGameStart;
    }

    private void OnDestroy()
    {
        cookingInputOutput.OnCookingStart -= OnCookingGameStart;
    }

    private void Update()
    {
        //CheckInput();
        SlowlyRotate();
    }

    private void OnCookingGameStart(FoodData foodCooked)
    {
        steakHeld = CookingInputOutputScript.SpawnDisplayFoodInPosition(foodCooked, flipObject.transform, localPositionOffset);

        SetTopAndBottom();
    }

    //public void CheckInput()
    //{
    //    //if (interactScript != null && !interactScript.withinRange)
    //    //{
    //    //    return;
    //    //}

    //    //if (steakHeld == null)
    //    //{
    //    //    return;
    //    //}

    //    //if (Input.GetKeyDown(KeyCode.T))
    //    //{
    //    //    FlipSteak(rotateAmount, Vector3.right);
    //    //}
    //}

    public void FlipSteak(float angle, Vector3 axis) 
    {
        desiredRotation = Quaternion.AngleAxis(angle, axis) * desiredRotation;

        sideGettingCooked = sideGettingCooked == topPart ? bottomPart : topPart;
        OnSteakFlipped?.Invoke(sideGettingCooked);
    }

    public void SlowlyRotate()
    {
        flipObject.transform.rotation = Quaternion.RotateTowards(flipObject.transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
    }

    private void SetTopAndBottom()
    {
        steakHeld = flipObject.transform.GetChild(0).gameObject;

        bottomPart = steakHeld.transform.Find("Bottom").gameObject;
        topPart = steakHeld.transform.Find("Top").gameObject;

        sideGettingCooked = bottomPart;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (interactScript != null && !interactScript.withinRange)
        {
            return;
        }

        if (steakHeld == null)
        {
            return;
        }

        FlipSteak(rotateAmount, Vector3.right);
    }
}
