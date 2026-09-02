using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections;

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

    private void OnDestroy()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(BeginSavingAllData());
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ClearAllData(); 
        }
    }

    public IEnumerator BeginSavingAllData()
    {
        if (currentlySavingGame)
        {
            yield return null;
        }

        gameData = new SaveData();

        OnSaveGame?.Invoke();

        currentlySavingGame = true;
        SaveGameData();
        yield return new WaitForSeconds(0.5f);
        currentlySavingGame = false;
    }

    public void BeginLoadingAllData()
    {
        gameData = LoadTheData();

        OnLoadSave?.Invoke();
    }

    public void SaveGameData()
    {
        SaveTheData(gameData);
    }

    public void SaveTheData(SaveData saveData)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + "/smt.lol";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        bf.Serialize(fileStream, saveData);

        fileStream.Close();
    }

    public SaveData LoadTheData()
    {
        string path = Application.persistentDataPath + "/smt.lol";

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fileStream = new FileStream(path, FileMode.Open);

            SaveData retrievedData = (SaveData)bf.Deserialize(fileStream);

            fileStream.Close();

            return retrievedData;
        }

        SaveData newSaveData = new SaveData();
        newSaveData.moneyAmount = 100;
        return newSaveData;
    }

    public void ClearAllData()
    {


        string path = Application.persistentDataPath + "/smt.lol";

        if (!File.Exists(path))
        {
            return;
        }
        File.Delete(path);
    }
}
