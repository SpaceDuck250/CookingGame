using UnityEngine;
using System.Collections.Generic;

public class RecipeBookScript : Interactable
{
    public List<RecipeData> normalRecipeDataList = new List<RecipeData>();

    public List<SpecialRecipe> specialRecipeDataList = new List<SpecialRecipe>();

    public bool opened = false;

    public GameObject closedBook;
    public GameObject openBook;

    public override void Interact(PlayerHandScript playerHand)
    {
        if (opened)
        {
            CloseBook();
        }
        else
        {
            OpenBook();

        }


    }

    public void OpenBook()
    {
        opened = true;
        openBook.SetActive(true);
        closedBook.SetActive(false);
    }

    public void CloseBook()
    {
        opened = false;
        openBook.SetActive(false);
        closedBook.SetActive(true);

    }
}
