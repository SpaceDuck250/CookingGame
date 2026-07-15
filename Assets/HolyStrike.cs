using UnityEngine;

public class HolyStrike : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    public float speed = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
            rb.linearVelocity = Vector3.down * speed;
            audioSource.enabled = true;
        }
    }
}