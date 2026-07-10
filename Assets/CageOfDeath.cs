

using UnityEngine;

public class CageOfDeath : MonoBehaviour
{
    RatAIScript rat;
    BoxCollider ratCollider;
    Vector3 deadRat = new Vector3(0,0,180);
    void Start()
    {
        rat = GameObject.Find("Mouse").GetComponent<RatAIScript>();
        ratCollider = GameObject.Find("Mouse").GetComponent<BoxCollider>();
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
        ratCollider.isTrigger = false;
        Destroy(gameObject);
    }
}
