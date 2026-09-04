using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    public GameObject carrot;
    public GameObject carrotSlices;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.E))
            {
                carrot.SetActive(true);
            }
            if(Input.GetKey(KeyCode.C))
            {
                carrot.SetActive(false);
                carrotSlices.SetActive(true);
            }
        }
    }
    }
