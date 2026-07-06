using UnityEngine;

public class SteakBottomCheckerScript : MonoBehaviour
{
    public LayerMask sideLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.up, out hit, float.MaxValue, sideLayer))
            {
                print(hit.collider.gameObject.name);
            }
        }
    }
}
