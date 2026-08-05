using UnityEngine;
using TMPro;

public class SnitchFloatingTextScript : MonoBehaviour
{
    public TextMeshPro textComponent;
    public float floatSpeed = 1f;
    public float lifeTime = 1.5f;

    private float timer = 0f;
    private Color startColor;

    private void Awake()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshPro>();
        }

        if (textComponent == null)
        {
            Debug.LogWarning("SnitchFloatingTextScript: no TextMeshPro found or assigned.");
            enabled = false;
            return;
        }

        textComponent.alignment = TextAlignmentOptions.Center;
        startColor = textComponent.color;
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        float fadeAmount = Mathf.Clamp01(1f - (timer / lifeTime));
        textComponent.color = new Color(startColor.r, startColor.g, startColor.b, fadeAmount);

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string newText, Color color)
    {
        textComponent.text = newText;
        textComponent.color = color;
        startColor = color;
    }
}