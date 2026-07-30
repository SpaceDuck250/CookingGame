using UnityEngine;

public class PlayerLooker : MonoBehaviour
{
    public static ILookable currentLookComponent;
    public Camera cam;

    public float maxDistance;

    private void Update()
    {
        TryLookAt();
        print(currentLookComponent);
    }

    private void TryLookAt()
    {
        int layerMask = ~(1 << 10);

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance, layerMask, QueryTriggerInteraction.Collide))
        {

            if (hit.collider == null)
            {
                return;
            }

            ILookable lookObj = hit.collider.GetComponentInParent<ILookable>();
            if (lookObj == null)
            {
                if (currentLookComponent != null)
                {
                    currentLookComponent.StopLookEffect();

                }
                currentLookComponent = null;
                return;
            }

            if (currentLookComponent != null)
            {
                currentLookComponent.StopLookEffect();

            }
            currentLookComponent = lookObj;
            currentLookComponent.DoLookEffect();
        }

        
    }

}