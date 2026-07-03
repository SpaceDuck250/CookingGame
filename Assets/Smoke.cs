using UnityEngine;

public class Smoke : MonoBehaviour
{
    public SteakSideScript top;
    public SteakSideScript bottom;
    public GameObject smoke;
        void Update()
    {
        if(top.burnt == true || bottom.burnt == true)
        {
            smoke.SetActive(true);
        }
    }
}
