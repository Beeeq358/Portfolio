using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPowerup : Powerup
{
    public override void Apply()
    {
        base.Apply();
        gameManager.destroyTimer *= 2;
        gameManager.bulletCooldown *= 2;
        gameManager.isRocket = true;
    }
}
