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
    GameObject obj = Instantiate(
        foodPrefab,
        heldObjectContainer.position,
        heldObjectContainer.rotation);
}
}