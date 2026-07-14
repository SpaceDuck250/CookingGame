using UnityEngine;
using TMPro;

public class FrierTempUIScript : MonoBehaviour
{
    public FrierInteractScript frierScript;
    public TextMeshProUGUI heatText;
    
    private void Start()
    {
        frierScript.OnChangeHeatLevel += ChangeHeatUI;
    }

    private void OnDestroy()
    {
        frierScript.OnChangeHeatLevel -= ChangeHeatUI;

    }

    public void ChangeHeatUI(FrierInteractScript.HeatLevel heatLevel)
    {
        heatText.text = "(" + heatLevel.name + ")";
        heatText.color = heatLevel.displayColor;
    }
}
