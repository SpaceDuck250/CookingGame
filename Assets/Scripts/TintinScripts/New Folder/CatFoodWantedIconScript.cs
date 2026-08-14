using UnityEngine;
using UnityEngine.UI;
using Cat;

public class CatFoodWantedIconScript : MonoBehaviour
{
    public CatAIScript catAi;

    // The object to show/hide - put GeneralBillBoard on this so it always faces the player
    public GameObject iconRoot;
    public Image foodIcon;

    private void Awake()
    {
        // Subscribe in Awake, not Start - BeginVisit fires the first state change
        // immediately after Instantiate, before any Start() calls have run
        catAi.OnCatChangeState += UpdateIconForState;
        iconRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        catAi.OnCatChangeState -= UpdateIconForState;
    }

    private void UpdateIconForState(CatState newState)
    {
        // Only show what it wants while it's actually waiting to be fed
        if (newState != CatState.Waiting)
        {
            iconRoot.SetActive(false);
            return;
        }

        if (catAi.requiredFood != null && catAi.requiredFood.foodSprite != null)
        {
            foodIcon.sprite = catAi.requiredFood.foodSprite;
        }

        iconRoot.SetActive(true);
    }
}