using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadshotPowerup : Powerup
{
    public override void Apply()
    {
        base.Apply();
        if (gameManager.spreadshot == Spreadshot.single)
        {
            gameManager.spreadshot = Spreadshot.twins;
        }
        else if (gameManager.spreadshot == Spreadshot.twins)
        {
            gameManager.spreadshot = Spreadshot.trishot;
        }
        else if (gameManager.spreadshot == Spreadshot.trishot)
        {
            gameManager.spreadshot = Spreadshot.quad;
        }
        else if (gameManager.spreadshot == Spreadshot.quad)
        {
            gameManager.spreadshot = Spreadshot.pentashot;
        }
        else
        {
            Debug.LogError("No Spreadshot Type Found");
        }
    }
}
