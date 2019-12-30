using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerData
{

    public int PlayerLives;
    public int PlayerShootingLevel;

    public PlayerData(int playerLives, int playerShootingLevel)
    {
        PlayerLives = playerLives;
        PlayerShootingLevel = playerShootingLevel;
    }

}
