using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Movement
{
    private bool slowDown = false;
    private bool airbrakeL = false;
    private bool airbrakeR = false;
    private float moveFloat = 0;
    private float airbrakeLeftMoveFloat = 0;
    private float airbrakeRightMoveFloat = 0;
    private bool isThrust;
    private Controls input = null;
    private int shipChoice = 1;

    [Header("Ship Stats")]
    [SerializeField] private float turningRateShip1;
    [SerializeField] private float turningRateShip2;
    [SerializeField] private float airbrakeStrengthShip1;
    [SerializeField] private float airbrakeStrengthShip2;
    [SerializeField] private float thrustShip1;
    [SerializeField] private float thrustShip2;
    [SerializeField] private float weightShip1;
    [SerializeField] private float weightShip2;
    [SerializeField] private float basePitchShip1;
    [SerializeField] private float basePitchShip2;

    [Header("Current Ship Stats")]
    public float shipTurningRate;
    public float shipAirbrakeStrength;
    public float shipThrust;
    public float shipWeight;

    [Header("Other")]
    [SerializeField] private GameObject particle;
    [SerializeField] private PlayerPreferences player1;
    private void Awake()
    {
        shipChoice = player1.shipChoice;
        GetComponent<EngineController>().shipChoice = shipChoice;
        if (shipChoice == 1)
        {
            ship1Model.SetActive(true);
            ship2Model.SetActive(false);
            shipTurningRate = turningRateShip1;
            shipAirbrakeStrength = airbrakeStrengthShip1;
            shipThrust = thrustShip1;
            shipWeight = weightShip1;
        }
        else if (shipChoice == 2)
        {
            ship1Model.SetActive(false);
            ship2Model.SetActive(true);
            shipTurningRate = turningRateShip2;
            shipAirbrakeStrength = airbrakeStrengthShip2;
            shipThrust = thrustShip2;
            shipWeight = weightShip2;
        }
        else
        {
            Debug.LogError("Ship choice value is not valid! shipChoice: " + shipChoice);
        }
    }
    protected override void Start()
    {
        base.Start();
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

    protected override void FixedUpdate()
    {

        if (!isThrust || airbrakeR || airbrakeL)
        {
            slowDown = true;
        }
        else
        {
            slowDown = false;
        }
        transform.Rotate((moveFloat + (airbrakeRightMoveFloat * shipAirbrakeStrength) + (airbrakeLeftMoveFloat * shipAirbrakeStrength)) * rotationSpeed * Vector3.up);
        if (isThrust)
        {
            rb.AddForce(shipThrust * thrust * transform.forward);

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

        base.FixedUpdate();
    }

    protected override void Boost()
    {
        base.Boost();
        StartCoroutine(EmittingTimer());
    }

    protected override void PastHoverHeight()
    {
        weight = shipWeight;
        base.PastHoverHeight();
    }

    private void OnMovePerformed(InputAction.CallbackContext value)
    {
        float inputValue = value.ReadValue<float>();
        moveFloat = inputValue * shipTurningRate;
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
