using UnityEngine;
using System.Collections;
using TMPro;

public class NumberChangeEffectScript
{
    public NumberChangeEffectScript(ref TextMeshProUGUI textToWriteInto)
    {
        textToChange = textToWriteInto;
    }

    private TextMeshProUGUI textToChange;
    //public string number => textToChange.t;

    public bool running = false;

    public IEnumerator DoNumberChangeEffect(float newValue, float oldValue, float waitTime)
    {
        running = true;

        int countBy = (newValue - oldValue) >= 0 ? 1 : -1;

        int countTimes = Mathf.FloorToInt(newValue) - Mathf.FloorToInt(oldValue) - 1;
        // 4.2 -> 8.7 would be 4 -> 7

        textToChange.text = oldValue.ToString();
        float cummulative = oldValue;

        for (int i = 0; i < countTimes; i++)
        {
            cummulative += countBy;
            textToChange.text = cummulative.ToString();

            yield return new WaitForSeconds(waitTime);
        }

        textToChange.text = newValue.ToString();

        running = false;
    }
}
