using System;
using TMPro;
using UnityEngine;

public class ClickPreviewScript : MonoBehaviour
{
    public string previewText;
    public char clickCharacter;

    public GameObject previewObject;
    public TextMeshProUGUI previewTextComp;
    public TextMeshProUGUI previewCharComp;

    public event Action<GameObject> OnPreviewShown;
    public event Action OnPreviewHidden;

    public InteractAreaScript interactArea;

    private void Awake()
    {
        Subscribe();
    }

    private void Start()
    {
        SetupPreviewObj();
    }

    private void OnDestroy()
    {
        if (interactArea != null)
        {
            interactArea.OnPlayerEnterRange -= OnPlayerEnterRange;
            interactArea.OnPlayerExitRange -= OnPlayerExitRange;
        }
    }

    public void Subscribe()
    {
        if (interactArea == null) return;

        interactArea.OnPlayerEnterRange += OnPlayerEnterRange;
        interactArea.OnPlayerExitRange += OnPlayerExitRange;
    }

    private void OnPlayerEnterRange(GameObject player)
    {
        ShowClickPreview(true);
        OnPreviewShown?.Invoke(player);
    }

    private void OnPlayerExitRange()
    {
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