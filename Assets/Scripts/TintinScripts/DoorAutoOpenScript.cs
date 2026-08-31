using UnityEngine;
using UnityEngine.AI;

public class DoorAutoOpenScript : MonoBehaviour
{
    public DoorScript door;

    private void Awake()
    {
        if (door == null)
        {
            door = GetComponentInParent<DoorScript>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NavMeshAgent>() == null)
        {
            return;
        }

        if (!door.isOpen)
        {
            door.OpenDoor();
        }
    }
}