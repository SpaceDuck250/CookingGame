using UnityEngine;

public class RotationBlocker : MonoBehaviour
{
    public TurnScript rotator;

    public Transform leftCheckPoint;
    public Transform rightCheckPoint;

    public Camera cam;

    public float maxDistance = 1f;

    private void Update()
    {
        CheckIfTheresWallOnSide(leftCheckPoint, -cam.transform.right, ref rotator.lockXNeg);
        CheckIfTheresWallOnSide(rightCheckPoint, cam.transform.right, ref rotator.lockXPos);
    }

    public void CheckIfTheresWallOnSide(Transform CheckPoint, Vector3 rayDirection, ref bool lockBool)
    {
        RaycastHit hit;

        Ray newRay = new Ray(CheckPoint.position, rayDirection);
        if (Physics.Raycast(newRay, out hit, maxDistance))
        {
            if (hit.collider.gameObject.GetComponent<Collider>().isTrigger)
            {
                return;
            }

            StopRotationInDirection(ref lockBool, true);
        }
        else
        {
            StopRotationInDirection(ref lockBool, false);

        }

    }

    public void StopRotationInDirection(ref bool lockBool, bool lockIt)
    {
        lockBool = lockIt;
    }

}
