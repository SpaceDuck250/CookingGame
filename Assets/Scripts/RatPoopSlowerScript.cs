using UnityEngine;

public class RatPoopSlowerScript : MonoBehaviour
{
    public PlayerMovement playerMove;

    public float originalSpeed;

    public float slowedSpeed;

    private void Start()
    {
        RatPoopScript.OnStepOnRatPoop += OnStepOnRatPoop;
    }

    private void OnDestroy()
    {
        RatPoopScript.OnStepOnRatPoop -= OnStepOnRatPoop;

    }

    public void OnStepOnRatPoop()
    {
        StopAllCoroutines();
        originalSpeed = playerMove.moveSpeed;

        playerMove.moveSpeed = slowedSpeed;

        Invoke("ReturnToNormalSpeed", 2);
    }

    public void ReturnToNormalSpeed()
    {
        playerMove.moveSpeed = originalSpeed;
    }

}
