using UnityEngine;
using System.Collections.Generic;

public class BirdMovementScript : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public float closeEnoughValue;

    public Transform currentFlyPoint;

    public Animator birdAnimator;

    public void FlyToPoint(Transform newFlyPoint)
    {
        birdAnimator.SetBool("Flying", true);

        currentFlyPoint = newFlyPoint;
        transform.position = Vector3.MoveTowards(transform.position, newFlyPoint.position, moveSpeed * Time.deltaTime);
        RotateTowardsDestination();
    }

    private void RotateTowardsDestination()
    {
        Vector3 rotateVector = (currentFlyPoint.position - transform.position).normalized;
        float rotateAngle = Mathf.Atan2(rotateVector.x, rotateVector.z) * Mathf.Rad2Deg;

        float offset = 180f;
        Quaternion endRotation = Quaternion.Euler(0, rotateAngle + offset, 0);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, rotateSpeed * Time.deltaTime);
    }


    public bool CheckIfCloseEnough()
    {
        float distance = Vector3.Distance(transform.position, currentFlyPoint.position);
        if (distance <= closeEnoughValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // This is not weird i promise
    public static void FillListWithChildrenFromTransform(Transform parent, ref List<Transform> listItGoesInto)
    {
        foreach (Transform child in parent)
        {
            listItGoesInto.Add(child);
        }
    }
}
