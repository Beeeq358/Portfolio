using UnityEngine;

public class CPUMovement : Movement
{
    [Header("AI Stats")]
    [SerializeField] private float navMeshVisionDistance;
    [SerializeField] private float navMeshPilotAssistDistance;
    [SerializeField] private float pilotAssistFactor;
    [SerializeField] private float pilotAssistThreshold;
    private float aiSpeed;
    private float count;
    private float thrustFactor = 1;
    private float aiFramesStuck = 200;
    public GameObject navMeshAgent;
    private Vector3 respawnPos;
    private Vector3 oldPos;
    private Vector3 newPos;
    private Quaternion respawnRot = Quaternion.Euler(0, 0, 0);

    protected override void FixedUpdate()
    {
        Vector3 agentForward = navMeshAgent.transform.forward;
        Vector3 futurePosition = navMeshAgent.transform.position + agentForward * navMeshVisionDistance;
        Vector3 targetDirection = (futurePosition - transform.position).normalized;

        aiSpeed = Vector3.Distance(oldPos, newPos);
        oldPos = newPos;
        newPos = gameObject.transform.position;

        targetDirection.y = 0;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (aiSpeed < 0.2f)
            {
                if (count < aiFramesStuck)
                    count++;
                else
                {
                    ResetPos();
                    count = 0;
                }
            }
        }

        rb.AddForce(transform.forward * thrustFactor * thrust);

        if (transform.position.y < 25)
            ResetPos();

        base.FixedUpdate();
    }

    private void ResetPos()
    {
        transform.position = respawnPos;
        rb.velocity = Vector3.zero;
        transform.rotation = respawnRot;
        rb.angularVelocity = Vector3.zero;
        navMeshAgent.GetComponent<AINavMesh>().AIRespawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Respawn" || other.gameObject.tag == "StartFinishLine")
        {
            respawnPos = other.transform.position;
            respawnRot = other.transform.rotation;
        }

        if (other.gameObject.tag == "StartFinishLine")
        {
            navMeshAgent.GetComponent<AINavMesh>().ChooseNewPath(true, 2);
        }

        if (other.gameObject.tag == "Out Of Bounds")
        {
            ResetPos();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SlowZone")
        {
            if (thrustFactor > 0.5f)
                thrustFactor -= 0.05f;
        }
        else
        {
            thrustFactor = 1;
        }
    }
}