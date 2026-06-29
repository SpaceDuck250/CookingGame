using UnityEngine;
using System.Collections.Generic;

public class PlatterGiverScript : MonoBehaviour
{
    public PlatterScript platterScript;

    public List<FoodData> GiveFoodDataListFromPlatter()
    {
        return platterScript.foodHeldList;
    }
}
