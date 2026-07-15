using UnityEngine;

public class FoodBox : Interactable
{
    public GameObject foodPrefab;
    public Transform heldObjectContainer;
    public override void Interact(PlayerHandScript playerHand)
{
    if (foodPrefab == null)
    {
        return;
    }

    if (heldObjectContainer == null)
    {
        return;
    }
    if(this.name != "Dabao Boxes")
        {
            GameObject obj = Instantiate(
        foodPrefab,
        heldObjectContainer.position,
        heldObjectContainer.rotation);
        }
    else if(heldObjectContainer.Find("Chicken Rice(Clone)") != null)
        {
            GameObject obj = Instantiate(
        foodPrefab,
        heldObjectContainer.position,
        heldObjectContainer.rotation);
        Destroy(heldObjectContainer.Find("Chicken Rice(Clone)").gameObject);
        playerHand.currentFoodHeld = null;
        playerHand.currentFoodHeldObj = null;
        }
}
}