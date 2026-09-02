using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    private int cameraMovementSpeed = 5;
    private float movementSmoothing = 10f;
    private float lookSensitivity = 3f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;

    public Camera m_Camera;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationY = rot.y;
        rotationX = rot.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * lookSensitivity;
        rotationX -= Input.GetAxis("Mouse Y") * lookSensitivity;

        rotationX = Mathf.Clamp(rotationX, -85f, 85f);
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.forward * vertical) + (transform.right * horizontal);

        if (Input.GetKey(KeyCode.E))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            moveDirection += Vector3.down;
        }

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        targetVelocity = moveDirection * cameraMovementSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, movementSmoothing * Time.deltaTime);

        transform.position += currentVelocity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
