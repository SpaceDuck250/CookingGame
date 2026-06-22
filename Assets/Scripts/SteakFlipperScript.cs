using UnityEngine;
using System;

public class SteakFlipperScript : MonoBehaviour
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

    private void Start()
    {
        desiredRotation = Quaternion.Euler(0, 0, 90);

        // Maybe change this later when making machine
        SetTopAndBottom();
    }

    private void Update()
    {
        CheckInput();
        SlowlyRotate();
    }

    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            FlipSteak(rotateAmount, Vector3.right);
        }
    }


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

        bottomPart = steakHeld.transform.GetChild(1).gameObject;
        topPart = steakHeld.transform.GetChild(2).gameObject;

        sideGettingCooked = bottomPart;
    }
}
