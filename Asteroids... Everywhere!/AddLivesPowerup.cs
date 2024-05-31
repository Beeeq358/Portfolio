using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLivesPowerup : Powerup
{
    public override void Apply()
    {
        base.Apply();
        gameManager.NextRound();
        gameManager.lives += 2;
    }
}
