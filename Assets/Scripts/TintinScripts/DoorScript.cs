using UnityEngine;

public class DoorScript : Interactable
{
    public bool isOpen = false;

    public Transform doorTransform;

    public float openAngle = 90f;
    public float rotateSpeed = 3f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Quaternion targetRotation;

    public LookControlsScript lookControlsScript;

    private void Awake()
    {
        closedRotation = doorTransform.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;

        targetRotation = closedRotation;
    }

    private void Update()
    {
        doorTransform.localRotation = Quaternion.RotateTowards(doorTransform.localRotation, targetRotation, rotateSpeed * 100f * Time.deltaTime);
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        targetRotation = isOpen ? openRotation : closedRotation;

        lookControlsScript.customControlsText = isOpen ? "Left Click (Close)" : "Left Click (Open)";

    }

    public void OpenDoor()
    {
        isOpen = true;
        targetRotation = openRotation;
    }

    public void CloseDoor()
    {
        isOpen = false;
        targetRotation = closedRotation;

    }
}