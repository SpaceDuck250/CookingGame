using UnityEngine;

public class PoopInteraction : Interactable
{
    public GameObject broom;
    public GameObject heldObject;
    void Update()
    {
        broom = GameObject.Find("Broom");
        heldObject = GameObject.Find("HeldObjectContainer");
    }
    public override void Interact(PlayerHandScript playerHand)
    {
        if(broom.transform.IsChildOf(heldObject.transform))
        {
            Destroy(gameObject);
        }
    }
}
