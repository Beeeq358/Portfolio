using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerMovement : MonoBehaviour
{
    public float hoverHeight;
    public LayerMask groundLayer;
    public LayerMask boostPad;
    [SerializeField] private GameObject particle;
    [SerializeField] private float magForce;

    private float antiGravityForce;
    [SerializeField] private float thrust;
    [SerializeField] private float rotationSpeed;
    private float newPlayerPosition;
    private float oldPlayerPosition;
    private float playerSpeed;
    private bool slowDown = false;
    private bool airbrakeL = false;
    private bool airbrakeR = false;
    // private float rotationAmount = 10f;
    [SerializeField] private float gravity;
    private float moveFloat = 0;
    private float airbrakeLeftMoveFloat = 0;
    private float airbrakeRightMoveFloat = 0;
    private bool isThrust;

    private Controls input = null;




    private Rigidbody rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        particle.SetActive(false);
    }

    private void OnEnable()
    {
        input = new Controls();
        input.Player.Turn.Enable();
        input.Player.Turn.performed += OnMovePerformed;
        input.Player.Turn.canceled += OnMoveCancelled;

        input.Player.RearView.Enable();
        input.Player.RearView.started += OnRearViewPerformed;
        input.Player.RearView.canceled += OnRearViewCancelled;

        input.Player.Thrust.Enable();
        input.Player.Thrust.started += OnThrustPerformed;
        input.Player.Thrust.canceled += OnThrustCancelled;

        input.Player.AirbrakeLeft.Enable();
        input.Player.AirbrakeLeft.performed += OnAirbrakeLeftPerformed;
        input.Player.AirbrakeLeft.canceled += OnAirbrakeLeftCancelled;

        input.Player.AirbrakeRight.Enable();
        input.Player.AirbrakeRight.performed += OnAirbrakeRightPerformed;
        input.Player.AirbrakeRight.canceled += OnAirbrakeRightCancelled;
    }

    private void OnDisable()
    {
        input.Player.Turn.Disable();
        input.Player.Turn.performed -= OnMovePerformed;
        input.Player.Turn.canceled -= OnMoveCancelled;

        input.Player.RearView.Disable();
        input.Player.RearView.performed -= OnRearViewPerformed;
        input.Player.RearView.canceled -= OnRearViewCancelled;

        input.Player.Thrust.Disable();
        input.Player.Thrust.performed -= OnThrustPerformed;
        input.Player.Thrust.canceled -= OnThrustCancelled;

        input.Player.AirbrakeLeft.Disable();
        input.Player.AirbrakeLeft.performed -= OnAirbrakeLeftPerformed;
        input.Player.AirbrakeLeft.canceled -= OnAirbrakeLeftCancelled;

        input.Player.AirbrakeRight.Disable();
        input.Player.AirbrakeRight.performed += OnAirbrakeRightPerformed;
        input.Player.AirbrakeRight.canceled += OnAirbrakeRightCancelled;
    }

    private void FixedUpdate()
    {

        if (!isThrust || airbrakeR || airbrakeL)
        {
            slowDown = true;
        }
        else
        {
            slowDown = false;
        }
        transform.Rotate(Vector3.up * (moveFloat + airbrakeRightMoveFloat + airbrakeLeftMoveFloat) * rotationSpeed);
        if (isThrust)
        {
            rb.AddForce(transform.forward * thrust);

            if (airbrakeL && !airbrakeR)
            {
                rb.AddForce(0.4f * (-airbrakeLeftMoveFloat) * thrust * -transform.forward);
            }
            else if (airbrakeR && !airbrakeL)
            {
                rb.AddForce(0.4f * airbrakeRightMoveFloat * thrust * -transform.forward);
            }
            else if (airbrakeL && airbrakeR)
            {
                Debug.Log("Anti Reverse Brakes");
                rb.AddForce(-transform.forward * thrust);
            }
        }




        /*
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed);
            //rb.AddTorque(transform.up * rotationSpeed);
        }
        

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * rotationSpeed);
            //rb.AddTorque(transform.up * -rotationSpeed);
        }
        */
        /*
        if (Input.GetKey(KeyCode.RightArrow))
        {
            slowDown = true;
            airbrakeR = true;
            //rb.AddForce(transform.right * 1f * thrust);
            //rb.MovePosition(rb.position + (transform.right * 0.3f));
            transform.Rotate(Vector3.up * 1.4f * rotationSpeed);
            //rb.AddTorque(transform.up * 1.4f * rotationSpeed);
            if (Input.GetKey(KeyCode.W))
            {
                if (!Input.GetKey(KeyCode.LeftArrow))
                {
                    rb.AddForce(-transform.forward * 0.3f * thrust);

                }
                else
                {
                    Debug.Log("Anti Reverse Brakes");
                    rb.AddForce(-transform.forward * 0.5f * thrust);
                }
            }
        } */
        /* else if(!Input.GetKey(KeyCode.RightArrow))
        {
            airbrakeR = false;
        } */


        /*if (Input.GetKey(KeyCode.LeftArrow))
        {
            //airbrakeL = true;
            //rb.AddForce(-transform.right * 1f * thrust);
            //rb.MovePosition(rb.position + (-transform.right * 0.3f));
            transform.Rotate(-Vector3.up * 1.4f * rotationSpeed);
            //rb.AddTorque(-transform.up * 1.4f * rotationSpeed);
            if (isThrust)
            {
                if (!Input.GetKey(KeyCode.RightArrow))
                {
                    rb.AddForce(-transform.forward * 0.3f * thrust);
                }
                else
                {
                    Debug.Log("Anti Reverse Brakes");
                    rb.AddForce(-transform.forward * 0.5f * thrust);
                   
                }
            }
            
        }
        */

        /* else if (!Input.GetKey(KeyCode.LeftArrow))
        {
            airbrakeL = false;
        } */


        newPlayerPosition = rb.transform.position.x + rb.transform.position.z;
        playerSpeed = newPlayerPosition - oldPlayerPosition;
        //Debug.Log(transform.localPosition.z);
        oldPlayerPosition = newPlayerPosition;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, boostPad))
        {
           if (hit.distance < hoverHeight * 2)
            {
                rb.AddForce(transform.forward * 5 * thrust);
                StartCoroutine(EmittingTimer());
            }
        }
        else if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            float distanceToGround = hoverHeight - hit.distance;
            antiGravityForce = distanceToGround * Physics.gravity.magnitude;
            if (hit.distance < hoverHeight)
            {
                rb.AddForce(Vector3.up * antiGravityForce * magForce, ForceMode.Force);
            }
            else if (hit.distance > hoverHeight)
            {
                rb.AddForce(Vector3.down * gravity);
            }
        }

        /* 
        if (airbrakeL && !airbrakeR)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAmount));
        }
        else if (airbrakeR && !airbrakeL)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -rotationAmount));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        } */
    }

    private void OnMovePerformed(InputAction.CallbackContext value)
    {
        float inputValue = value.ReadValue<float>();
        moveFloat = inputValue;
    }
    private void OnMoveCancelled(InputAction.CallbackContext value)
    {
        moveFloat = 0;
    }

    private void OnThrustPerformed(InputAction.CallbackContext context)
    {
        isThrust = context.ReadValueAsButton();
    }
    private void OnThrustCancelled(InputAction.CallbackContext context)
    {
        isThrust = context.ReadValueAsButton();
    }

    public void OnRearViewPerformed(InputAction.CallbackContext context)
    {
        CameraController.isRearView = context.ReadValueAsButton();
    }
    public void OnRearViewCancelled(InputAction.CallbackContext context)
    {
        CameraController.isRearView = context.ReadValueAsButton();
    }


    public void OnAirbrakeLeftPerformed(InputAction.CallbackContext value)
    {
        float inputValue = value.ReadValue<float>();
        airbrakeLeftMoveFloat = inputValue * -1.4f;
        airbrakeL = true;
    }
    public void OnAirbrakeLeftCancelled(InputAction.CallbackContext value)
    {
        airbrakeLeftMoveFloat = 0;
        airbrakeL = false;
    }

    public void OnAirbrakeRightPerformed(InputAction.CallbackContext value)
    {
        float inputValue = value.ReadValue<float>();
        airbrakeRightMoveFloat = inputValue * 1.4f;
        airbrakeR = true;
    }
    public void OnAirbrakeRightCancelled(InputAction.CallbackContext value)
    {
        airbrakeRightMoveFloat = 0;
        airbrakeR = false;
    }

    IEnumerator EmittingTimer()
    {
        GetComponentInChildren<TrailRenderer>().emitting = true;
        particle.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        GetComponentInChildren<TrailRenderer>().emitting = false;
        yield return new WaitUntil(() => slowDown);
        particle.SetActive(false);
    }
}
