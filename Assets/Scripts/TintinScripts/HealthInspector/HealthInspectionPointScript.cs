using UnityEngine;
using System.Collections.Generic;

public class HealthInspectionPointScript : MonoBehaviour
{
    public string pointName = "Inspection Point";

    public Transform standPoint;

    public float checkRadius = 1.5f;

    public List<string> violationTags = new List<string> { "HealthViolation" };

    public bool CheckForViolation(out string violationDescription)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius);

        List<string> foundViolations = new List<string>();
        foreach (Collider hit in hits)
        {
            if (!violationTags.Contains(hit.tag))
            {
                continue;
            }

            string label = hit.gameObject.name;
            if (!foundViolations.Contains(label))
            {
                foundViolations.Add(label);
            }
        }

        if (foundViolations.Count <= 0)
        {
            violationDescription = "";
            return false;
        }

        violationDescription = foundViolations[0];
        if (foundViolations.Count > 1)
        {
            violationDescription += " (+" + (foundViolations.Count - 1) + " more)";
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}