using UnityEngine;
using System.Collections.Generic;

namespace RecipeBook
{
    public class RecipeBookScript : Interactable
    {
        public List<RecipeData> normalRecipeDataList = new List<RecipeData>();

        public List<SpecialRecipe> specialRecipeDataList = new List<SpecialRecipe>();

        public List<Page> pageList = new List<Page>();

        public bool opened = false;

        public GameObject closedBook;
        public GameObject openBook;

        public TurnScript turnScript;
        public Transform lookPoint;

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

            turnScript.LockCameraToPoint(lookPoint.transform.position, Quaternion.Euler(new Vector3(61f, 0, 0)), transform);
        }

        public void CloseBook()
        {
            opened = false;
            openBook.SetActive(false);
            closedBook.SetActive(true);

            turnScript.ReturnBackToPlayer();
        }

    }

    public class Page
    {
        public RecipeData[] recipeArray = new RecipeData[3];
    }
}
