
using UnityEngine;

public class CageOfDeath : MonoBehaviour
{
    RatAIScript rat;
    void Start()
    {
        rat = GameObject.Find("Mouse").GetComponent<RatAIScript>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Rat"))
        {
            rat.enabled = false;
        }
    }
    void OnDisable()
    {
        rat.enabled = true;
        Destroy(gameObject);
    }
}
