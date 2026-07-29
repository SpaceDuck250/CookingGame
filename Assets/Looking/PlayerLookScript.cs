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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cam.transform.forward, out hit, maxDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
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