using UnityEngine;

public class Sizzle : MonoBehaviour
{
    public GameObject stick;
    public AudioSource sizzle;

    // Update is called once per frame
    void Update()
    {
        if(stick.transform.childCount > 0)
        {
            sizzle.enabled = true;
        }
        else
        {
            sizzle.enabled = false;
        }
    }
}
