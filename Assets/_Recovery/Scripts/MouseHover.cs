using UnityEngine;

public class MouseHover : MonoBehaviour
{
    GameObject currentObject;
    public static Vector2 controllerMousePosition;
    public static bool usingController = false;

    void Update()
    {
        Vector2 mousePosition;

        if (usingController)
        {
            mousePosition = controllerMousePosition;
        }
        else
        {
            mousePosition = Input.mousePosition;
        }
        

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Only interact with objects tagged "Interactable"
            if (hitObject.CompareTag("Easy Box") ||
            hitObject.CompareTag("Normal Box") ||
            hitObject.CompareTag("Hard Box") ||
            hitObject.CompareTag("Jovan Box")||
            hitObject.CompareTag("Recipe Book") || 
            hitObject.CompareTag("Instructions") || 
            hitObject.CompareTag("Map")|| 
            hitObject.CompareTag("Pointer"))
            {
                if (hitObject != currentObject)
                {
                    RemoveGlow();

                    currentObject = hitObject;

                    MeshRenderer renderer = currentObject.GetComponentInChildren<MeshRenderer>();

                    if (renderer != null && renderer.material.color != Color.green)
                    {
                        Material mat = renderer.material;
                        mat.EnableKeyword("_EMISSION");
                        mat.SetColor("_EmissionColor", Color.yellow * 5f);
                    }
                }
            }
            else
            {
                RemoveGlow();
            }
        }
        else
        {
            RemoveGlow();
        }
    }
    void RemoveGlow()
    {
        if (currentObject != null)
        {
            MeshRenderer renderer = currentObject.GetComponentInChildren<MeshRenderer>();

            if (renderer != null && renderer.material.color != Color.green)
            {
                renderer.material.SetColor("_EmissionColor", Color.black);
            }

            currentObject = null;
        }
    }
}