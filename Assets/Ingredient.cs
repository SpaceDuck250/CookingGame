using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public int chopsRequired = 3;

    int chops = 0;
    public GameObject choppedPrefab;
    Transform choppedIngredientsParent;
    void Start()
    {
        choppedIngredientsParent = GameObject.Find("Place Area").transform;
    }
    public void Chop()
    {
        chops++;

        Debug.Log("Chopped " + gameObject.name);

        if (chops >= chopsRequired)
        {
            Debug.Log(gameObject.name + " is fully chopped!");

            Instantiate(
                choppedPrefab,
                transform.position,
                transform.rotation,
                choppedIngredientsParent);

            Destroy(gameObject);
        }
    }
}
