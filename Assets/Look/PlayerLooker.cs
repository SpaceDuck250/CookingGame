using UnityEngine;

public class PlayerLooker : MonoBehaviour
{
    public ILookable currentLookObj;
    public Camera cam;

    public float maxDistance;

    private void Update()
    {
        TryLookAt();
        print(currentLookObj);
    }

    private void TryLookAt()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cam.transform.forward, out hit, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider != null)
            {
                ILookable lookObj = hit.collider.GetComponentInParent<ILookable>();
                if (lookObj == null)
                {
                    if (currentLookObj != null)
                    {
                        currentLookObj.StopLookEffect();

                    }
                    currentLookObj = null;
                    return;
                }

                if (currentLookObj != null)
                {
                    currentLookObj.StopLookEffect();

                }
                currentLookObj = lookObj;
                currentLookObj.DoLookEffect();
            }
        }
    }
}
