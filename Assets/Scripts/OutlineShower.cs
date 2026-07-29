using UnityEngine;
using System.Collections.Generic;

public class OutlineShower : MonoBehaviour, ILookable
{
    public List<Renderer> renderersList = new List<Renderer>();

    private void Start()
    {
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
        Renderer selfRenderer = gameObject.GetComponent<Renderer>();
        if (selfRenderer != null)
        {
            renderersList.Add(selfRenderer);
        }

        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                renderersList.Add(childRenderer);
            }
        }
    }

    public void ShowOutline(bool show)
    {
        if (renderersList.Count == 0)
        {
            return;
        }

        foreach (Renderer rend in renderersList)
        {
            if (rend != null)
            {
                rend.renderingLayerMask = show ? (1u << 3) : (1u << 0);
            }
        }
    }


}
