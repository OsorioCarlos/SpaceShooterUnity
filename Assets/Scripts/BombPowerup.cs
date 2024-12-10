using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerup : Powerup
{
    public override void UsePowerup(GameObject gameObjectPlayer)
    {
        gameObjectPlayer.GetComponent<Player>().IncreaseBombs();
    }
}
