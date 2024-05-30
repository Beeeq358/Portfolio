using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject MenuBG;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject endSessionButton;
    [SerializeField] private GameObject backgroundResults;
    [SerializeField] private GameObject exitButtonResults;
    [SerializeField] private GameObject mainMenuButtonResults;
    [SerializeField] private GameObject bestLapTime;
    [SerializeField] private GameObject bestLapResults;
    [SerializeField] private GameObject lastLapTime;
    [SerializeField] private GameObject currentTime;
    [SerializeField] private GameObject leaderboard;

    public bool isMenuEnabled = false;
    public bool isResultsScreen = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuEnabled = !isMenuEnabled;
        }




        if (isMenuEnabled && !isResultsScreen)
        {
            MenuBG.SetActive(true);
            resumeButton.SetActive(true);
            endSessionButton.SetActive(true);
        }
        else if (!isMenuEnabled)
        {
            MenuBG.SetActive(false);
            resumeButton.SetActive(false);
            endSessionButton.SetActive(false);
        }

        if (isResultsScreen)
        {
            isMenuEnabled = false;

            backgroundResults.SetActive(true);
            exitButtonResults.SetActive(true);
            bestLapResults.SetActive(true);
            mainMenuButtonResults.SetActive(true);
            leaderboard.SetActive(true);

            lastLapTime.SetActive(false);
            currentTime.SetActive(false);
            bestLapTime.SetActive(false);
        }
        else if (!isResultsScreen)
        {
            backgroundResults.SetActive(false);
            bestLapResults.SetActive(false);
            exitButtonResults.SetActive(false);
            mainMenuButtonResults.SetActive(false);
            leaderboard.SetActive(false);
        }
    }

    
}
