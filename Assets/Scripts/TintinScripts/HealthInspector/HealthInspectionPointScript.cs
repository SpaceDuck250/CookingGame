using UnityEngine;
using System.Collections.Generic;

public class HealthInspectionPointScript : MonoBehaviour
{
    public string pointName = "Inspection Point";

    public Transform standPoint;

    public float checkRadius = 1.5f;

    public List<string> violationTags = new List<string> { "HealthViolation" };

    // Returns every individual violating object found at this point (not deduped),
    // so callers can both count total violations and tally them by name.
    public bool CheckForViolation(out List<string> foundViolations)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius);

        foundViolations = new List<string>();
        foreach (Collider hit in hits)
        {
            if (!violationTags.Contains(hit.tag))
            {
                continue;
            }

            if (IsProperlyPlated(hit))
            {
                continue;
            }

            foundViolations.Add(CleanViolationLabel(hit.gameObject.name));
        }

        return foundViolations.Count > 0;
    }

    // Food sitting correctly on a platter shouldn't count as a violation, even if it's tagged
    private bool IsProperlyPlated(Collider hit)
    {
        HoldableFoodScript holdable = hit.GetComponentInParent<HoldableFoodScript>();
        return holdable != null && holdable.platterIn != null;
    }

    // Strips Unity's "(Clone)" suffix so spawned violation props show a clean name in reports
    private const string CloneSuffix = "(Clone)";

    private string CleanViolationLabel(string objectName)
    {
        if (!objectName.EndsWith(CloneSuffix))
        {
            return objectName;
        }

        int cleanLength = objectName.Length - CloneSuffix.Length;
        return objectName.Substring(0, cleanLength).Trim();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}