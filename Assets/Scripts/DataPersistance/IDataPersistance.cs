public interface IDataPersistance
{
    void LoadData(GameStates data);
    void SaveData(ref GameStates data);
}
