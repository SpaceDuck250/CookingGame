using RecipeBook;
using UnityEngine;

public class RecipeBookUIScript : MonoBehaviour
{
    public GameObject crossHair, moneyBar;

    public Transform pageContentsContainer;

    public GameObject recipeItemTemplate;

    public Sprite cuttingStationSprite, panStationSprite, skewerStationSprite;

    public RecipeBookScript recipeBookScript;

    public Animator pageAnimator;

    public LookControlsScript lookControls;

    private void Start()
    {
        recipeBookScript.OnBookOpen += OnBookOpen;
        recipeBookScript.OnBookClose += OnBookClose;
    }

    private void OnDestroy()
    {
        recipeBookScript.OnBookOpen -= OnBookOpen;
        recipeBookScript.OnBookClose -= OnBookClose;
    }

    public void GeneratePage(Page pageData)
    {
        ClearContainer();

        foreach (RecipeData recipeItem in pageData.recipeArray)
        {
            GameObject newRecipeItem = Instantiate(recipeItemTemplate, pageContentsContainer);

            Sprite cookingStationSprite = FindIconForCookingUsed(recipeItem.cookingStationUsed);
            newRecipeItem.GetComponent<RecipeDataItemScript>().SetupRecipeItem(recipeItem, cookingStationSprite);
        }
    }

    public void GenerateLatestPage()
    {
        GeneratePage(recipeBookScript.pageList[recipeBookScript.currentShownPageIndex]);
    }

    // 1 or -1
    public void FlipPage(int amount)
    {
        recipeBookScript.OnPageTurn?.Invoke();

        recipeBookScript.currentShownPageIndex += amount;
        if (recipeBookScript.currentShownPageIndex < 0)
        {
            recipeBookScript.currentShownPageIndex = 0;
            return;
        }
        else if (recipeBookScript.currentShownPageIndex >= recipeBookScript.pageList.Count)
        {
            recipeBookScript.currentShownPageIndex = recipeBookScript.pageList.Count - 1;
            return;
        }

        // Will play anim then generate a new page
        if (amount > 0)
        {
            pageAnimator.SetTrigger("TurnForward");
        }
        else
        {
         
            pageAnimator.SetTrigger("TurnBack");
        }
    }

    public void ClearContainer()
    {
        foreach (Transform child in pageContentsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public Sprite FindIconForCookingUsed(CookingStation cookingStation)
    {
        switch (cookingStation)
        {
            case CookingStation.Cut:
                return cuttingStationSprite;

            case CookingStation.Pan:
                return panStationSprite;

            case CookingStation.Skewer:
                return skewerStationSprite;
            default:
                return null;

        }
    }

    public void OnBookClose()
    {

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        crossHair.SetActive(true);
        moneyBar.SetActive(true);

        if (lookControls != null)
        {
            lookControls.customControlsText = "Left Click (Open Book)";

        }

    }

    public void OnBookOpen()
    {

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;

        crossHair.SetActive(false);
        moneyBar.SetActive(false);

        if (lookControls != null)
        {
            lookControls.customControlsText = "Esc (Close Book) \n < and > (Turn Page)";

        }

    }
}
