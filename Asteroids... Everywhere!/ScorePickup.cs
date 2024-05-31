using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : Pickup
{
    public int amount;
    public override void Activate()
    {
        base.Activate();
        amount = 5000 * game.currentRound;
        GameManager.score += amount;
    }
}
