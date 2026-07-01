using UnityEngine;
using UnityEngine.UIElements;

public class RatPoopSpawner : MonoBehaviour
{
    public RatAIScript ratAi;

    public GameObject ratPoop;
    public Transform backTransform;

    private void Start()
    {
        ratAi.OnMouseStopEating += OnMouseStopEating;
    }

    private void OnDestroy()
    {
        ratAi.OnMouseStopEating -= OnMouseStopEating;
    }


    public void OnMouseStopEating(Vector3 foodPos)
    {

        Vector3 downOffset = Vector3.down * 0.1f;
        Instantiate(ratPoop, backTransform.position + downOffset, ratPoop.transform.rotation);
    }
}
