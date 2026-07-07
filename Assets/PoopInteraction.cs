using UnityEngine;

public class PoopInteraction : Interactable
{
    public GameObject broom;
    void Update()
    {
        broom = GameObject.Find("Broom");
    }
    public override void Interact(PlayerHandScript playerHand)
    {
        if(broom.activeSelf == true)
        {
            Destroy(gameObject);
        }
    }
}
