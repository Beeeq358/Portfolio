using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFirePowerup : Powerup
{
    public override void Apply()
    {
        base.Apply();
        gameManager.bulletCooldown -= 0.2f;
        if (gameManager.bulletCooldown < 0)
        {
            gameManager.bulletCooldown = 0;
            Debug.Log("Bullet Cooldown is 0 and cant be lowered");
        }
        else
        {
            Debug.Log("Bullet Cooldown down");
        }

    }
}