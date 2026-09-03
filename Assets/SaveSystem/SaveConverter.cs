using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SaveConverter : MonoBehaviour
{
    public GameObject[] AllFoodModels;

    public BrownFoodBox[] AllFoodBoxes;

    public Dictionary<int, GameObject> FoodToIDMap = new Dictionary<int, GameObject>();
    public Dictionary<int, BrownFoodBox> FoodBoxToIDMap = new Dictionary<int, BrownFoodBox>();

    public static SaveConverter instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        FillMapping<GameObject>(AllFoodModels, FoodToIDMap);
        FillMapping<BrownFoodBox>(AllFoodBoxes, FoodBoxToIDMap);
    }

    public void FillMapping<T>(T[] list, Dictionary<int, T> mapping)
    {
        if (list.Length == 0)
        {
            return;
        }

        int mapId = 0;

        foreach (T item in list)
        {
            mapping.Add(mapId, item);
            mapId++;
        }
    }

    public static int MapItemToId<T>(T item, Dictionary<int, T> mapping)
    {
        int id = mapping.FirstOrDefault(n => EqualityComparer<T>.Default.Equals(item, n.Value)).Key;

        return id;
    }

    public static T MapIDToItem<T>(int id, Dictionary<int, T> mapping)
    {
        if (mapping.ContainsKey(id))
        {
            return mapping[id];
        }
        else
        {
            return default(T);
        }
    }

}
