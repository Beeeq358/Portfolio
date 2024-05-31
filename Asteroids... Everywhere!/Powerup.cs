using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    protected GameManager gameManager;
    public string displayName;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public virtual void Apply()
    {
        gameManager.powerupButton1.SetActive(false);
        gameManager.powerupButton2.SetActive(false);
        gameManager.NextRound();
    }
}
