using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
public class CharacterChange : MonoBehaviour
{
    public Slider slider;
    public GameObject foodBar;
    public GameObject carrotSlices;
    public GameObject carrot;
    public GameObject knife;
    public GameObject pan;
    public float fillRate = 0.5f;
    public Animator animator;
    public RuntimeAnimatorController knifeCutting;
    public RuntimeAnimatorController panFry;
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
                knife.SetActive(true);
                animator.runtimeAnimatorController = knifeCutting;
                foodBar.SetActive(true);
                slider.value += fillRate * Time.deltaTime;
            }
            else if(Input.GetKey(KeyCode.Q))
            {
                pan.SetActive(true);
                animator.runtimeAnimatorController = panFry;
                foodBar.SetActive(true);
                slider.value += fillRate * Time.deltaTime;
            }
            else
            {
                knife.SetActive(false);
                pan.SetActive(false);
                foodBar.SetActive(false);
                slider.value = 0;
                animator.runtimeAnimatorController = null;
            }
        }
        else
        {
            knife.SetActive(false);
            pan.SetActive(false);
            foodBar.SetActive(false);
            slider.value = 0;
        }
        if(slider.value >= 1)
        {
            knife.SetActive(false);
            animator.runtimeAnimatorController = null;
            slider.value = 0;
            foodBar.SetActive(false);
            carrot.SetActive(false);
            carrotSlices.SetActive(true);
        }
    }
}
