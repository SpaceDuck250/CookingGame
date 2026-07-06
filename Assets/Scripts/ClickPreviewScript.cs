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

<<<<<<< HEAD
=======
    private void Awake()
    {
        Subscribe();
    }

>>>>>>> origin/newestAlex
    private void Start()
    {
        SetupPreviewObj();

<<<<<<< HEAD
        interactArea.OnPlayerEnterRange += OnPlayerEnterRange;
        interactArea.OnPlayerExitRange += OnPlayerExitRange;
=======
        
>>>>>>> origin/newestAlex
    }

    private void OnDestroy()
    {
        interactArea.OnPlayerEnterRange -= OnPlayerEnterRange;
        interactArea.OnPlayerExitRange -= OnPlayerExitRange;
    }

<<<<<<< HEAD
=======
    public void Subscribe()
    {
        interactArea.OnPlayerEnterRange += OnPlayerEnterRange;
        interactArea.OnPlayerExitRange += OnPlayerExitRange;
    }

>>>>>>> origin/newestAlex
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
