using UnityEngine;

public class FloorHitterScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            other.gameObject.GetComponent<FloorManager>().OnFoodHitThisFloor?.Invoke(gameObject);
            print("Hit floor");
        }
    }

    private void OnDestroy()
    {
        FloorManager.OnFoodPickupFromFloor?.Invoke(gameObject);
        print("Left floor");
    }
}
