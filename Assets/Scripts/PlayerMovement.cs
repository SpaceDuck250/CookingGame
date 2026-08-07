using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    public Camera cam;

    public float moveSpeed;
    public float smoothValue;

    public float moveX;
    public float moveZ;

    public Vector3 xLook;
    public Vector3 zLook;

    private Vector3 refVelocity;

    public bool canMove = true;

    public Action OnMove;
    public Action OnStopMove;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        xLook = cam.transform.right.normalized;
        xLook.y = 0;

        zLook = cam.transform.forward.normalized;
        zLook.y = 0;

        CheckIfMoving(moveX, moveZ);

    }

    private void FixedUpdate()
    {
        //Vector3 targetVelocity = new Vector3(moveX, 0, moveZ) * moveSpeed; // Change later
        Vector3 targetVelocity = zLook * moveZ + xLook * moveX;
        targetVelocity *= moveSpeed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref refVelocity, smoothValue * Time.deltaTime);


    }

    public void FreezeMovement(bool value)
    {
        rb.constraints = value ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.FreezeRotation;
    }

    public void CheckIfMoving(float moveX, float moveZ)
    {
        if (moveX == 0 && moveZ == 0)
        {
            OnStopMove?.Invoke();
        }
        else
        {
            OnMove?.Invoke();
        }
    }
}
