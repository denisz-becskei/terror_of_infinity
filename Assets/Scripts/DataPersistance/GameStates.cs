using UnityEngine;
using System.Collections.Generic;

public class GameStates
{
    public int GAME_SEED;
    public Dictionary<string, int> PICKED_UP_BITS;
    public int NUMBER_OF_LIVES;
    public string PLAYER_WORLD_COORDINATES;
    public string PLAYER_NAME;
    public long PLAYER_SCORE;

    public GameStates() {
        GAME_SEED = WorldWideScripts.GetPureRandomNumberBetween(0, int.MaxValue);
        PICKED_UP_BITS = new Dictionary<string, int>();
        NUMBER_OF_LIVES = 10;
        PLAYER_WORLD_COORDINATES = "0:0::0:0";
        PLAYER_NAME = "";
        PLAYER_SCORE = 0;
    }

    public GameStates(int number_of_lives, string player_name)
    {
        GAME_SEED = Random.Range(0, int.MaxValue);
        PICKED_UP_BITS = new Dictionary<string, int>()
        {
            {"BLACK", 0},
            {"BROWN", 0},
            {"ORANGE", 0},
            {"PURPLE", 0},
            {"MAGENTA", 0},
            {"BLUE", 0},
            {"GREEN", 0},
            {"YELLOW", 0},
            {"RED", 0},
            {"WHITE", 0}
        };
        NUMBER_OF_LIVES = number_of_lives;
        PLAYER_WORLD_COORDINATES = "0:0::0:0";
        PLAYER_NAME = player_name;
        PLAYER_SCORE = 0;
    }
}
