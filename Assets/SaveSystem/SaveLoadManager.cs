using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveData gameData;

    public static event Action OnSaveGame;
    public static event Action OnLoadSave;

    public static bool currentlySavingGame;

    private bool dataJustCleared = false;

    public static SaveLoadManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BeginLoadingAllData();
        dataJustCleared = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //StartCoroutine(BeginSavingAllData());
            //BeginSavingAllData();
            print(DaySystemManager.dayCounter + " and money is " + MoneyManager.playerMoneyAmount);
            print("L");
        }

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    ClearAllData();
        //    dataJustCleared = true;
        //}
    }

    public void BeginSavingAllData()
    {
        if (dataJustCleared)
        {
            return;
        }

        gameData = new SaveData();

        OnSaveGame?.Invoke();

        currentlySavingGame = true;
        SaveGameData();
        //yield return new WaitForSeconds(0f);
        currentlySavingGame = false;
    }

    public void BeginLoadingAllData()
    {
        gameData = LoadClass<SaveData>();

        OnLoadSave?.Invoke();
    }

    public void SaveGameData()
    {
        //SaveTheData(gameData);
        SaveClass<SaveData>(gameData);
    }

    public void SaveClass<T>(T dataClass)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + "/smt.lol";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        bf.Serialize(fileStream, dataClass);

        fileStream.Close();
    }

    public T LoadClass<T>()
    {
        string path = Application.persistentDataPath + "/smt.lol";

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fileStream = new FileStream(path, FileMode.Open);

            T retrievedData = (T)bf.Deserialize(fileStream);

            fileStream.Close();

            return retrievedData;
        }
        else
        {
            return default(T);
        }
    }

    public void ClearAllData()
    {
        string path = Application.persistentDataPath + "/smt.lol";

        //if (!File.Exists(path))
        //{
        //    return;
        //}
        //File.Delete(path);
        //dataJustCleared = true;

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        dataJustCleared = true;
    }

    private void OnApplicationQuit()
    {
        BeginSavingAllData();
    }
 



}
