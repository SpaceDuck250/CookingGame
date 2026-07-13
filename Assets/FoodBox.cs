using UnityEngine;

public class FoodBox : MonoBehaviour
{
    public GameObject[] ingredientPrefab;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(this.name == "Carrot Box")
            {
                ingredientPrefab[0].SetActive(true);
                ingredientPrefab[1].SetActive(false);
                ingredientPrefab[2].SetActive(false);
                ingredientPrefab[3].SetActive(false);
            }
            else if(this.name == "Eggplant Box")
            {
                ingredientPrefab[1].SetActive(true);
                ingredientPrefab[0].SetActive(false);
                ingredientPrefab[2].SetActive(false);
                ingredientPrefab[3].SetActive(false);
            }
            else if(this.name == "Rice Box")
            {
                ingredientPrefab[2].SetActive(true);
                ingredientPrefab[0].SetActive(false);
                ingredientPrefab[1].SetActive(false);
                ingredientPrefab[3].SetActive(false);
            }
            else if(this.name == "Steak Box")
            {
                ingredientPrefab[3].SetActive(true);
                ingredientPrefab[0].SetActive(false);
                ingredientPrefab[1].SetActive(false);
                ingredientPrefab[2].SetActive(false);
            }
        }
    }
}