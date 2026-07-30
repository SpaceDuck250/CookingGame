using UnityEngine;
using System.Collections.Generic;

public class OutlineShower : MonoBehaviour, ILookable
{
    public List<GameObject> objectsToOutlineList = new List<GameObject>();

    public bool setManually = false;

    private void Start()
    {
        if (setManually)
        {
            return;
        }

        FillRenderersList();
    }

    public void DoLookEffect()
    {
        ShowOutline(true);
    }
    
    public void StopLookEffect()
    {
        ShowOutline(false);
    }

    public void FillRenderersList()
    {
        objectsToOutlineList.Add(gameObject);

        if (transform.childCount == 0)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            objectsToOutlineList.Add(child.gameObject);
        }
    }

    public void ShowOutline(bool show)
    {
        if (objectsToOutlineList.Count == 0)
        {
            return;
        }

        if (objectsToOutlineList.Count == 0)
        {
            return;
        }

        foreach (GameObject obj in objectsToOutlineList)
        {
            if (obj == null)
            {
                continue;
            }

            obj.layer = show ? LayerMask.NameToLayer("Outlined") : LayerMask.NameToLayer("Default");
        }
    }


}
