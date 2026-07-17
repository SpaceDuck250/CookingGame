using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float interactDistance = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChopIngredient();
        }
    }

    void ChopIngredient()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Ingredient ingredient = hit.collider.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                ingredient.Chop();
            }
        }
    }
}
