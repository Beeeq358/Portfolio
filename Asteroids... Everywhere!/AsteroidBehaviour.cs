using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;

    private float rotationSpeed;

    private Rigidbody rb;
    private Quaternion randomRotation;
    private float randomForce;
    private float randomDirection;
    private bool antiSpawnKill;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        randomDirection = Random.Range(0, 360);
        rotationSpeed = Random.Range(-7, 7);



        randomForce = Random.Range(minForce, maxForce);

        randomRotation = Quaternion.Euler(0, randomDirection, 0);
        transform.rotation = randomRotation;

        rb.AddForce(transform.forward * randomForce, ForceMode.Impulse);
        StartCoroutine(AntiSpawnKillTimer());
    }

    private void Update()
    {
        if (transform.position.z < -12.5)
        {
            transform.position += new Vector3(0, 0, 24);
        }
        else if (transform.position.z > 12.5)
        {
            transform.position += new Vector3(0, 0, -24);
        }

        if (transform.position.x < -23.5)
        {
            transform.position += new Vector3(46, 0, 0);
        }
        else if (transform.position.x > 23.5)
        {
            transform.position += new Vector3(-46, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && antiSpawnKill)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotationSpeed);
    }

    IEnumerator AntiSpawnKillTimer()
    {
        antiSpawnKill = true;
        yield return new WaitForSeconds(0.3f);
        antiSpawnKill = false;
    }
}
