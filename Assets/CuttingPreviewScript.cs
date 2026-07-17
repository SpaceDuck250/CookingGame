using System;
using TMPro;
using UnityEngine;

public class CuttingPreviewScript : MonoBehaviour
{
    public TextMeshProUGUI textPreview;
    public GameObject placeArea;
    public ClickPreviewScript clickPreviewScript;

    private void Update()
    {
        if(placeArea.transform.childCount > 0)
        {
            clickPreviewScript.enabled = false;
            textPreview.text = "Cut Ingredients";
        }
        else
        {
            clickPreviewScript.enabled = true;
            textPreview.text = "Place Food";
        }
    }
}

