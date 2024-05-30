using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utilities : MonoBehaviour
{
    public MenuController controller;

    public void ResumePressed()
    {
        controller.isMenuEnabled = false;
    }

    public void EndSessionPressed()
    {
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
        SceneManager.LoadScene("Tracks");
    }
}
