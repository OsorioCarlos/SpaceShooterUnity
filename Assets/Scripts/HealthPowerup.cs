using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Powerup
{
    public override void UsePowerup(GameObject gameObjectPlayer)
    {
        gameObjectPlayer.GetComponent<Player>().IncreaseLifePoints();
    }
}
