using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ghost : ScriptableObject
{
    public bool isRecord;
    public bool isReplay;
    public float recordFrequency;


    public List<float> timeStamp;
    public List<Vector3> position;
    public List<Vector3> rotation;

    public float lapTime;
    public int shipChoice;

    public void ResetData()
    {
        timeStamp.Clear();
        position.Clear();
        rotation.Clear();
        lapTime = 0;
    }

    
}