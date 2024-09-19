using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public AudioSource engineSound, engineIdle, engineHigh, engineLow;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float volumeAmplifier, pitchAmplifier, idleVolume, idlePitch;
    private float maxPitch;
    private float maxShipPitch;
    public int shipChoice;
    private Vector3 oldPos;
    private Vector3 newPos;
    private float pitch, ignoreMaxPitch;
    private float volume;
    

    private void Awake()
    {
        maxPitch = 1.5f;
        if (shipChoice == 1)
        {
            pitchAmplifier = 1.6f;
            maxShipPitch = 1;
        }
        else if (shipChoice == 2)
        {
            pitchAmplifier = 1.2f;
            maxShipPitch = 0;
        }
    }
    private void FixedUpdate()
    {
        playerSpeed = Vector3.Distance(oldPos, newPos);
        oldPos = newPos;
        newPos = gameObject.transform.position;
        pitch = playerSpeed * pitchAmplifier;
        ignoreMaxPitch = pitch;
        volume = playerSpeed * volumeAmplifier;
        if (pitch <= 0.3f)
        {
            pitch = 0.3f;
            engineIdle.volume = idleVolume - 0.3f;
        }
        else
        {
            engineIdle.volume = 0;
        }

        if (pitch + maxShipPitch > maxPitch)
        {
            pitch = maxPitch;
        }
        engineSound.pitch = pitch + idlePitch;
        engineSound.volume = volume + idleVolume;
        engineLow.pitch = ignoreMaxPitch + idlePitch;
        engineLow.volume = volume + idleVolume + 0.3f;
        engineHigh.pitch = ignoreMaxPitch + idlePitch;
        engineHigh.volume = volume * 0.4f;
    }
}
