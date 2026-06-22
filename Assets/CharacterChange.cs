using UnityEngine;
using UnityEngine.UI;
public class CharacterChange : MonoBehaviour
{
    public Slider slider;
    public GameObject cuttingBar;
    public float fillRate = 0.5f;
    public Animator animator;
    public RuntimeAnimatorController runtimeAnimatorController;
    public LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.75f;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, 10f, layerMask))
        {
            Debug.Log("Hit: " + hit.collider.name);
            if(Input.GetKey(KeyCode.E))
            {
                animator.runtimeAnimatorController = runtimeAnimatorController;
                cuttingBar.SetActive(true);
                slider.value += fillRate * Time.deltaTime;
            }
            else
            {
                cuttingBar.SetActive(false);
                animator.runtimeAnimatorController = null;
            }
        }
    }
}
