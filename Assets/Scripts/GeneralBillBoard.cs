using UnityEngine;

public class GeneralBillBoard : MonoBehaviour
{
    private void Update()
    {
        RotateTowardsPlayer(PlayerHandScript.instance.gameObject, gameObject);
    }

    private void RotateTowardsPlayer(GameObject player, GameObject self)
    {
        Vector3 rotateVector = (player.transform.position - self.transform.position).normalized;
        float rotateAngle = Mathf.Atan2(rotateVector.x, rotateVector.z) * Mathf.Rad2Deg;

        float offset = 180;
        rotateAngle += offset;

        self.transform.rotation = Quaternion.Euler(0, rotateAngle, 0);
    }
}
