using UnityEngine;

public class TurnScript : MonoBehaviour
{
    public float mouseX;
    public float xTurn;

    public float mouseY;
    public float yTurn;

    public float senseX;
    public float senseY;

    public float yRange;

    public Camera cam;

    public Vector3 originalPosition;

    public Vector3 destination;
    public Quaternion newRotation;

    public float smoothValue;
    public float rotateSpeed;

    public PlayerMovement playerMove;

    public bool lockMode = false;

    public bool lockXNeg = false;
    public bool lockXPos = false;

    public bool canTurn = true;

    private void Start()
    {
        originalPosition = cam.transform.localPosition;
    }

    private void Update()
    {
        if (!canTurn)
        {
            return;
        }

        if (lockMode)
        {
            MoveCamera();
            return;
        }

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //xTurn += mouseX * Time.deltaTime * senseX;
        yTurn -= mouseY * Time.deltaTime * senseY;

        yTurn = Mathf.Clamp(yTurn, -yRange, yRange);

        cam.transform.localRotation = Quaternion.Euler(yTurn, 0, 0);



        //transform.Rotate(Vector3.up * mouseX * Time.deltaTime * senseX);
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * senseX);



    }

    public void LockCameraToPoint(Vector3 newPoint, Quaternion turnAngle, Transform newParent)
    {
        lockMode = true;

        playerMove.canMove = false;
        playerMove.FreezeMovement(true);

        cam.transform.parent = newParent;

        destination = newPoint;
        newRotation = turnAngle;

    }

    public void MoveCamera()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, destination, Time.deltaTime * smoothValue);
        cam.transform.localRotation = Quaternion.RotateTowards(cam.transform.localRotation, newRotation, Time.deltaTime * rotateSpeed);
    }

    public void ReturnBackToPlayer()
    {
        lockMode = false;

        cam.transform.parent = transform;

        cam.transform.localPosition = originalPosition;

        playerMove.canMove = true;

        playerMove.FreezeMovement(false);
    }

    public float TryLockDirection(float mouseX)
    {
        if (lockXNeg && mouseX < 0)
        {
            return 0;
        }
        else if (lockXPos && mouseX > 0)
        {
            return 0;
        }

        return mouseX;
    }
}
