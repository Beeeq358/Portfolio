using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirestormPickup : Pickup
{
    public override void Activate()
    {
        game.StartFireStorm();
        base.Activate();
    }


}   
