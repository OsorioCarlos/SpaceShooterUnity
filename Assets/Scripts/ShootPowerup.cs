using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPowerup : Powerup
{
    public override void UsePowerup(GameObject gameObjectPlayer)
    {
        gameObjectPlayer.GetComponent<Player>().IncreaseShootLevel();
    }
}
