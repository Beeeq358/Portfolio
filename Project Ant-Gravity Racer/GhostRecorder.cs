using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public Ghost ghost;
    private float timer;
    private float timeValue;

    public void StartRecording()
    {
        if (ghost.isRecord)
        {
            ghost.ResetData();
            timeValue = 0;
            timer = 0;
        }
    }


    private void Update()
    {
            timer += Time.unscaledDeltaTime;
            timeValue += Time.unscaledDeltaTime;


        if (ghost.isRecord & timer >= 1 / ghost.recordFrequency)
        {

            ghost.timeStamp.Add(timeValue);
            ghost.position.Add(this.transform.position);
            ghost.rotation.Add(this.transform.eulerAngles + new Vector3(90, 180, 0));
            
            timer = 0;
        }

    }
}
