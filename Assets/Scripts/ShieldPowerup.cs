using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : Powerup
{
    public override void UsePowerup(GameObject gameObjectPlayer)
    {
        gameObjectPlayer.GetComponent<Player>().TurnOnShield();
    }
}
