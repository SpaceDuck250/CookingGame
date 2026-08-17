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

        public int currentShownPageIndex = 0;

        public bool opened = false;

        public event Action OnBookOpen;
        public event Action OnBookClose;
        public Action OnPageTurn;

        public GameObject closedBook;
        public GameObject openBook;

        public GameObject crossHair;
        public GameObject moneyBar;

        public TurnScript turnScript;
        public Transform lookPoint;

        public RecipeBookUIScript recipeBookUI;

        private void Start()
        {
            AutoFillPageList();

            recipeBookUI.GeneratePage(pageList[currentShownPageIndex]);
        }

        private void Update()
        {
            if (!opened)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseBook();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                recipeBookUI.FlipPage(1);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                recipeBookUI.FlipPage(-1);
            }
        }

        public override void Interact(PlayerHandScript playerHand)
        {
            if (opened)
            {
                return;
            }

            OpenBook();
        }

        public void OpenBook()
        {
            OnBookOpen?.Invoke();

            opened = true;
            openBook.SetActive(true);
            closedBook.SetActive(false);

            // replace with event later


            turnScript.LockCameraToPoint(lookPoint.transform.position, Quaternion.Euler(new Vector3(60, 180, 0)), transform);
        }

        public void CloseBook()
        {
            OnBookClose?.Invoke();

            opened = false;
            openBook.SetActive(false);
            closedBook.SetActive(true);

            // replace with event later


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
