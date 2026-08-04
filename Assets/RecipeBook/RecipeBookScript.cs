using UnityEngine;
using System.Collections.Generic;
using System;

namespace RecipeBook
{
    public class RecipeBookScript : Interactable
    {
        public List<RecipeData> normalRecipeDataList = new List<RecipeData>();

        public List<SpecialRecipe> specialRecipeDataList = new List<SpecialRecipe>();

        public int maxItemsPerPage;
        public List<Page> pageList = new List<Page>();

        public bool opened = false;

        public GameObject closedBook;
        public GameObject openBook;

        public GameObject crossHair;

        public TurnScript turnScript;
        public Transform lookPoint;

        private void Start()
        {
            AutoFillPageList();
        }

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

            //OpenBook();
        }

        public void OpenBook()
        {
            opened = true;
            openBook.SetActive(true);
            closedBook.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            crossHair.SetActive(false);

            turnScript.LockCameraToPoint(lookPoint.transform.position, Quaternion.Euler(new Vector3(61f, 0, 0)), transform);
        }

        public void CloseBook()
        {
            opened = false;
            openBook.SetActive(false);
            closedBook.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            crossHair.SetActive(true);

            turnScript.ReturnBackToPlayer();
        }

        private void AutoFillPageList()
        {
            if (normalRecipeDataList.Count == 0)
            {
                return;
            }

            int currentPageIndex = 0;
            pageList.Add(new Page());

            foreach (RecipeData recipe in normalRecipeDataList)
            {
                pageList[currentPageIndex].recipeArray.Add(recipe);
                if (pageList[currentPageIndex].recipeArray.Count >= maxItemsPerPage)
                {
                    pageList.Add(new Page());
                    currentPageIndex++;
                }
            }
        }

    }

    [Serializable]
    public class Page
    {
        public List<RecipeData> recipeArray = new List<RecipeData>();
    }
}
