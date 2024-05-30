using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Child;
    [SerializeField] private GameObject LookAt;
    [SerializeField] private float speed;
    public static bool isRearView;
    public static CameraController instance;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Child = Player.transform.Find("Camera Constaint").gameObject;
        LookAt = Player.transform.Find("Camera LookAt").gameObject;

    }

    private void FixedUpdate()
    {
        Follow();
    }





    private void Follow()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, Child.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(LookAt.gameObject.transform.position);
        if (isRearView)
        {
            gameObject.transform.Rotate(25, 180, 0);
        }
    }
}
