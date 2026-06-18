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

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        xTurn += mouseX * Time.deltaTime * senseX;
        yTurn -= mouseY * Time.deltaTime * senseY;

        yTurn = Mathf.Clamp(yTurn, -yRange, yRange);

        transform.rotation = Quaternion.Euler(yTurn, xTurn, 0);

    }
}
