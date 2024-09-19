using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    public Ghost ghost;
    public int shipChoice = 1;
    private float timeValue;
    private int index1;
    private int index2;
    [SerializeField] private GameObject ship1Model, ship2Model;


    private void Awake()
    {
        StartPlaying();
    }
    private void Start()
    {
        shipChoice = ghost.shipChoice;
        if (shipChoice == 1)
        {
            ship1Model.SetActive(true);
            ship2Model.SetActive(false);
        }
        else if (shipChoice == 2)
        {
            ship1Model.SetActive(false);
            ship2Model.SetActive(true);
        }
        else
        {
            Debug.LogError("Ship choice value is not valid for Ghost! shipChoice: " + shipChoice);
        }
    }

    public void StartPlaying()
    {
        timeValue = 0;
    }

    

    private void Update()
    {
        timeValue += Time.unscaledDeltaTime;

        if (ghost.isReplay)
        {
            if (ghost.position.Count == 0) { return; }
            GetIndex();
            SetTransform();
        }
    }

    private void GetIndex()
    {
        
        for (int i = 0; i < ghost.timeStamp.Count - 2; i++)
        {
            if (ghost.timeStamp[i] == timeValue)
            {
                index1 = i;
                index2 = i;
                return;
            }
            else if (ghost.timeStamp[i] < timeValue & timeValue < ghost.timeStamp[i + 1])
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }

        index1 = ghost.timeStamp.Count - 1;
        index2 = ghost.timeStamp.Count - 1;
    }

    private void SetTransform()
    {
        if (index1 == index2)
        {
            this.transform.position = ghost.position[index1];
            this.transform.eulerAngles = ghost.rotation[index1];
        }
        else
        {
            float interpolationFactor = (timeValue - ghost.timeStamp[index1]) / (ghost.timeStamp[index2] - ghost.timeStamp[index1]);

            this.transform.position = Vector3.Lerp(ghost.position[index1], ghost.position[index2], interpolationFactor);
            this.transform.eulerAngles = Vector3.Lerp(ghost.rotation[index1], ghost.rotation[index2], interpolationFactor);
        }
    }
}
