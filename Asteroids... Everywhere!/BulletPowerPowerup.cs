using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPowerPowerup : Powerup
{
    public override void Apply()
    {
        base.Apply();
        gameManager.bulletSpeed += 1f;
        gameManager.destroyTimer += 0.1f;
    }
}