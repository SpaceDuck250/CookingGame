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

    private void Start()
    {
        BeginLoadingAllData();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //StartCoroutine(BeginSavingAllData());
            BeginSavingAllData();
            print("L");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ClearAllData(); 
        }
    }

    public void BeginSavingAllData()
    {
        //if (currentlySavingGame)
        //{
        //    yield return null;
        //}

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

    //public SaveData LoadTheData()
    //{
    //    string path = Application.persistentDataPath + "/smt.lol";

    //    if (File.Exists(path))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();

    //        FileStream fileStream = new FileStream(path, FileMode.Open);

    //        SaveData retrievedData = (SaveData)bf.Deserialize(fileStream);

    //        fileStream.Close();

    //        return retrievedData;
    //    }
    //    else
    //    {
    //        SaveData newSaveData = new SaveData();
    //        newSaveData.moneyAmount = 100;
    //        return newSaveData;
    //    }

           
    //}

    public void ClearAllData()
    {
        string path = Application.persistentDataPath + "/smt.lol";

        if (!File.Exists(path))
        {
            return;
        }
        File.Delete(path);
    }

    private void OnApplicationQuit()
    {
        BeginSavingAllData();
    }
}
