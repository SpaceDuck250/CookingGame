using UnityEngine;
using System.Collections.Generic;

public class HealthInspectionPointScript : MonoBehaviour
{
    public string pointName = "Inspection Point";

    public Transform standPoint;

    public float checkRadius = 1.5f;

    public List<string> violationTags = new List<string> { "HealthViolation" };

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

            foundViolations.Add(GetViolationLabel(hit));
        }

        return foundViolations.Count > 0;
    }

    private bool IsProperlyPlated(Collider hit)
    {
        HoldableFoodScript holdable = hit.GetComponentInParent<HoldableFoodScript>();
        return holdable != null && holdable.platterIn != null;
    }

    private string GetViolationLabel(Collider hit)
    {
        HoldableFoodScript holdable = hit.GetComponentInParent<HoldableFoodScript>();
        if (holdable != null && holdable.foodData != null && !string.IsNullOrEmpty(holdable.foodData.foodName))
        {
            return holdable.foodData.foodName;
        }

        return CleanViolationLabel(hit.gameObject.name);
    }

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