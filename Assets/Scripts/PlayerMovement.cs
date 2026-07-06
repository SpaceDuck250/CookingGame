using UnityEngine;

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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 
    private void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        xLook = cam.transform.right;
        xLook.y = 0;

        zLook = cam.transform.forward;
        zLook.y = 0;

    }

    private void FixedUpdate()
    {
        //Vector3 targetVelocity = new Vector3(moveX, 0, moveZ) * moveSpeed; // Change later
        Vector3 targetVelocity = zLook * moveZ + xLook * moveX;
        targetVelocity *= moveSpeed;
        targetVelocity.y = 0;

        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref refVelocity, smoothValue * Time.deltaTime);


    }
}
