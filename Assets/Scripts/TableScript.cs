using UnityEngine;

public class TableScript : MonoBehaviour
{
    private void Start()
    {
        SetupAllChairChildren();
    }

    public void SetupAllChairChildren()
    {
        foreach (Transform child in transform)
        {
            ChairScript chairScript = child.GetChild(0).GetComponent<ChairScript>();
            if (chairScript != null)
            {
                chairScript.tableParent = gameObject;
            }
        }
    }
}

