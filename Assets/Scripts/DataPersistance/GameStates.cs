using UnityEngine;
using System.Collections.Generic;

public class GameStates
{
    public int GAME_SEED;
    public List<string> PICKED_UP_BITS;
    public int NUMBER_OF_LIVES;
    public string PLAYER_WORLD_COORDINATES;

    public GameStates() {
        GAME_SEED = WorldWideScripts.GetTotallyRandomNumberBetween(0, int.MaxValue);
        PICKED_UP_BITS = new List<string>();
        NUMBER_OF_LIVES = 10;
        PLAYER_WORLD_COORDINATES = "0:0::0:0";
    }

    public GameStates(int number_of_lives)
    {
        GAME_SEED = Random.Range(0, int.MaxValue);
        PICKED_UP_BITS = new List<string>();
        NUMBER_OF_LIVES = number_of_lives;
        PLAYER_WORLD_COORDINATES = "0:0::0:0";
    }
}
