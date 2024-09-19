using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected float antiGravityForce, weight = 1;
    public float hoverHeight;
    public LayerMask groundLayer, boostPad;
    [SerializeField] protected float magForce, thrust, rotationSpeed, gravity;

    [SerializeField] protected GameObject ship1Model, ship2Model, ghostGO;
    protected Rigidbody rb;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, boostPad))
        {
            if (hit.distance < hoverHeight * 2)
            {
                Boost();
            }
        }
        else if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            float distanceToGround = hoverHeight - hit.distance;
            antiGravityForce = distanceToGround * Physics.gravity.magnitude;
            if (hit.distance < hoverHeight)
            {
                rb.AddForce(antiGravityForce * magForce * Vector3.up, ForceMode.Force);
            }
            else if (hit.distance > hoverHeight)
            {
                PastHoverHeight();
            }
        }
    }

    protected virtual void Boost()
    {
        rb.AddForce(5 * thrust * transform.forward);
    }
    protected virtual void PastHoverHeight()
    {
        rb.AddForce(gravity * weight * Vector3.down);
    }
}
