using UnityEngine;
using System;

public class RatPoopScript : MonoBehaviour
{
    public static event Action OnStepOnRatPoop;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnStepOnRatPoop?.Invoke();
            Destroy(gameObject);
            print("hit poop");
        }
    }
}
