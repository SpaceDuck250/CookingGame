using UnityEngine;

public class MeatWiggleScript : MonoBehaviour
{
    public float springStrength = 150f;
    public float damping = 8f;
    public float maxSquash = 0.15f;

    private Vector3 baseScale;
    private float velocity;
    private float offset;

    private void Start()
    {
        baseScale = transform.localScale;
        Bump(5f);
    }

    private void Update()
    {
        // Spring-damper toward 0
        float force = -springStrength * offset - damping * velocity;
        velocity += force * Time.deltaTime;
        offset += velocity * Time.deltaTime;

        float squash = 1f + Mathf.Clamp(offset, -maxSquash, maxSquash);
        transform.localScale = new Vector3(baseScale.x * (2f - squash), baseScale.y * squash, baseScale.z * (2f - squash));
    }

    public void Bump(float strength)
    {
        velocity += strength;
    }
}