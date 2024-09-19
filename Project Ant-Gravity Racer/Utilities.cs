using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utilities : MonoBehaviour
{
    public MenuController controller;
    public PlayerPreferences player1Preferences;
    [SerializeField] private GameObject Ghost;

    public void ResumePressed()
    {
        controller.isMenuEnabled = false;
    }

    public void EndSessionPressed()
    {
        SaveSystem.SaveGhost(new SaveData(Ghost.GetComponent<GhostPlayer>().ghost));
        controller.isResultsScreen = true;
        controller.isMenuEnabled = false;
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
        Debug.Log("Game Closed!");
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void StartPressed()
    {
        SceneManager.LoadScene("Ship Select");
    }


    public void Ship1Pressed()
    {
        player1Preferences.shipChoice = 1;
        SceneManager.LoadScene("Tracks");
    }

    public void Ship2Pressed()
    {
        player1Preferences.shipChoice = 2;
        SceneManager.LoadScene("Tracks");
    }
}
