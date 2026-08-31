using UnityEngine;
using UnityEngine.UI;
using Cat;

public class CatFoodWantedIconScript : MonoBehaviour
{
    public CatAIScript catAi;
    public GameObject iconRoot;
    public Image foodIcon;

    private void Awake()
    {
        catAi.OnCatChangeState += UpdateIconForState;
        iconRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        catAi.OnCatChangeState -= UpdateIconForState;
    }

    private void UpdateIconForState(CatState newState)
    {
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