using UnityEngine;
using System;

public class RatHeadStopper : MonoBehaviour
{
    //public event Action OnRatHitFood;

    public GameObject ratBody;

    public LayerMask foodLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == foodLayer)
        {

        }
    }
}
