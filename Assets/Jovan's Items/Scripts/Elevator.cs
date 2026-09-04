using UnityEngine;

public class Elevator : MonoBehaviour
{
    float speed = 8;
    float width = 16;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }
    void FixedUpdate()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * width;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
