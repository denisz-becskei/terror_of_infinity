using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameStates gameStates;
    private FileDataHandler dataHandler;
    private List<IDataPersistance> dataPersistanceObjects;
    public static DataPersistanceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistance Manager in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public GameStates GetGameState()
    {
        return gameStates;
    }

    public void NewGame()
    {
        gameStates = new GameStates();
    }

    public void LoadGame()
    {
        gameStates = dataHandler.Load();

        if (gameStates == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach(IDataPersistance dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(gameStates);
        }
    }

    public void SaveGame()
    {
        foreach(IDataPersistance dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.SaveData(ref gameStates);
        }

        dataHandler.Save(gameStates);
    }

    public void ReformatGame()
    {
        gameStates = new GameStates(gameStates.NUMBER_OF_LIVES);
        dataHandler.Save(gameStates);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
