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

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //xTurn += mouseX * Time.deltaTime * senseX;
        yTurn -= mouseY * Time.deltaTime * senseY;

        yTurn = Mathf.Clamp(yTurn, -yRange, yRange);

        cam.transform.localRotation = Quaternion.Euler(yTurn, 0, 0);

        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * senseX);

    }
}
