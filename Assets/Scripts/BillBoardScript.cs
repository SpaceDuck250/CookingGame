using UnityEngine;

public class BillBoardScript : MonoBehaviour
{
    public GameObject player;
    public GameObject self;

    public InteractAreaScript interactScript;

    private void Start()
    {
        interactScript.OnPreviewShown += OnPreviewShown;

        gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        interactScript.OnPreviewShown -= OnPreviewShown;
    }

    private void OnPreviewShown(GameObject player)
    {
        this.player = player;
    }

    private void Update()
    {
        RotateTowardsPlayer(player, self);
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
