using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : Powerup
{
    public static int shieldLives = 2;
    public override void Apply()
    {
        base.Apply();
        gameManager.isShield = true;
    }
}
