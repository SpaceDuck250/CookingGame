using TMPro;
using UnityEngine;
using System;

public class CookingInteractScript : MonoBehaviour
{
    public string previewText;
    public char clickCharacter;

    public GameObject previewObject;
    public TextMeshProUGUI previewTextComp;
    public TextMeshProUGUI previewCharComp;

    public event Action<GameObject> OnPreviewShown;
    public event Action OnPreviewHidden;


    private void Start()
    {
        SetupPreviewObj();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        OnPreviewShown?.Invoke(other.gameObject);
        ShowClickPreview(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        ShowClickPreview(false);
        OnPreviewHidden?.Invoke(); 
    }

    private void SetupPreviewObj()
    {
        previewTextComp.text = previewText;
        previewCharComp.text = clickCharacter.ToString();
    }

    private void ShowClickPreview(bool value)
    {
        previewObject.SetActive(value);
    }
}
