using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{
    [SerializeField] private bool infiniteHealth;

    private void Start()
    {
        int lives = GameObject.FindGameObjectWithTag("DataPersistance").GetComponent<DataPersistanceManager>().GetGameState().NUMBER_OF_LIVES;
        if (infiniteHealth && lives < 5)
        {
            GameObject.FindGameObjectWithTag("DataPersistance").GetComponent<DataPersistanceManager>().GetGameState().NUMBER_OF_LIVES = 5;
        }
    }
}
