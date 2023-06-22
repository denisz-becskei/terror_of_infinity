using UnityEngine;

public class CheatController : MonoBehaviour, IDataPersistance
{
    [SerializeField] bool infiniteHealth;

    public void LoadData(GameStates data)
    {
        return;
    }

    public void SaveData(ref GameStates data)
    {
        if(infiniteHealth)
        {
            data.NUMBER_OF_LIVES = 10;
        }
    }
}
