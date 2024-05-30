using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    private bool lapCompleted = false;
    private bool lapInvalid = false;
    private bool startLap = false;
    private float lapTime = 0;
    private int lapTimeInt = 0;
    private bool checkpoint1Clear = false;
    private bool checkpoint2Clear = false;
    private bool T3checkpoint1Clear = false;
    private bool T3checkpoint2Clear = false;
    private bool T3checkpoint3Clear = false;
    private bool T3checkpoint4Clear = false;
    private float storedLapTime = 0;
    private float bestLapTime = 0;
    private Vector3 respawnPos;
    private Quaternion respawnRot = Quaternion.Euler(0, 0, 0);
    public Ghost ghost;
    public Ghost bestLapGhost;
    public GhostPlayer ghostPlayer;
    private GhostRecorder ghostRecorder;



    [SerializeField] private Text lapTimeText;
    [SerializeField] private Text lastLapText;
    [SerializeField] private Text bestLapText;
    [SerializeField] private Text bestLapResults;
    [SerializeField] private TextMeshProUGUI bestLapFloat;

    private Rigidbody rb;
    private void Start()
    {
        ghostRecorder = GetComponent<GhostRecorder>();

        bestLapGhost.isRecord = false;
        ghost.isRecord = false;
        bestLapGhost.isReplay = false;
        ghost.isReplay = false;
        ghost.ResetData();
        bestLapGhost.ResetData();

        rb = GetComponent<Rigidbody>();

        respawnPos = new Vector3(372, 53.5f, 363);
    }

    private void FixedUpdate()
    {
        if (startLap)
        {
            lapTime = Time.fixedDeltaTime + lapTime;
        }
    }
    private void Update()
    {

        lapTimeInt = Mathf.RoundToInt(lapTime * 1000);
        lapTimeText.text = (lapTimeInt / 1000f).ToString("F2");
        lastLapText.text = "Last Lap: " + (storedLapTime / 1000f).ToString("F2");
        bestLapText.text = "Best Lap: " + (bestLapTime / 1000f).ToString("F2");
        bestLapFloat.text = bestLapTime.ToString();


        if (bestLapText.text == "Best Lap: 0.00")
            bestLapText.text = "Best Lap: No Time";

        if (lastLapText.text == "Last Lap: 0.00")
            lastLapText.text = "Last Lap: No Time";

        bestLapResults.text = bestLapText.text;
        //Debug.Log("lapTime: " + lapTime);
        //Debug.Log("startLap: " + startLap);

        if (checkpoint1Clear && checkpoint2Clear && startLap)
            lapCompleted = true;
        else if (T3checkpoint1Clear &&  T3checkpoint2Clear && T3checkpoint3Clear && T3checkpoint4Clear)
            lapCompleted = true;

        if (transform.position.y < 25)
            ResetPos();
    }

    private void ResetPos()
    {
        transform.position = respawnPos;
        rb.velocity = Vector3.zero;
        transform.rotation = respawnRot;
        rb.angularVelocity = Vector3.zero;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.gameObject.tag == "StartFinishLine")
        {
            respawnPos = other.transform.position;
            respawnRot = other.transform.rotation;



            if (!lapInvalid && lapCompleted)
            {
                storedLapTime = lapTimeInt;
                if (lapTimeInt < bestLapTime || bestLapTime == 0)
                {
                    bestLapTime = lapTimeInt;
                    bestLapGhost.position = new List<Vector3>(ghost.position);
                    bestLapGhost.rotation = new List<Vector3>(ghost.rotation);
                    bestLapGhost.timeStamp = new List<float>(ghost.timeStamp);

                }
            }
            else
            {
                lapInvalid = false;
            }
            bestLapGhost.isReplay = true;
            ghost.isRecord = true;
            ghostRecorder.StartRecording();
            ghostPlayer.StartPlaying();
            startLap = true;
            lapCompleted = false;
            lapTime = 0;
            checkpoint1Clear = false;
            checkpoint2Clear = false;
            T3checkpoint1Clear = false;
            T3checkpoint2Clear = false;
            T3checkpoint3Clear = false;
            T3checkpoint4Clear = false;
        }

        if (other.gameObject.tag == "Checkpoint" && startLap)
        {
            if ((other.gameObject.name == "Checkpoint1" || other.gameObject.name == "Checkpoint1 (1)") || other.gameObject.name == "Checkpoint1 (2)")
                if (checkpoint2Clear == false)
                {
                    {
                        checkpoint1Clear = true;
                    }
                }
                else
                {
                    Debug.Log("Wrong way! Incorrect order of checkpoints!");
                    lapInvalid = true;

                }

            else if (other.gameObject.name == "Checkpoint2" || other.gameObject.name == "Checkpoint2 (1)" || other.gameObject.name == "Checkpoint2 (2)")
            {
                checkpoint2Clear = true;
            }
            else
            {
                Debug.LogError("No Valid Checkpoint Found");
                lapInvalid = true;
            }
        }

        if (other.gameObject.CompareTag("T3Checkpoint") && startLap)
        {
            if (other.gameObject.name == "Checkpoint1")
            {
                T3checkpoint1Clear = true;
            }
            else if (other.gameObject.name == "Checkpoint2" && T3checkpoint1Clear)
            {
                T3checkpoint2Clear = true;
            }
            else if (other.gameObject.name == "Checkpoint3" && T3checkpoint2Clear)
            {
                T3checkpoint3Clear = true;
            }
            else if (other.gameObject.name == "Checkpoint4" && T3checkpoint3Clear)
            {
                T3checkpoint4Clear = true;
            }
            else
            {
                lapInvalid = true;
            }
        }






        if (other.gameObject.tag == "Out Of Bounds")
        {
            ResetPos();
        }

        if (other.gameObject.tag == "Respawn")
        {
            respawnPos = other.transform.position;
            respawnRot = other.transform.rotation;
        }
    }
}
