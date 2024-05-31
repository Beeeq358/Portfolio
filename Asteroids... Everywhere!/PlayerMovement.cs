using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private float rotationSpeed;
    private Rigidbody rb;
    private MeshRenderer mr;
    private Transform boostLightL;
    private Transform boostLightR;
    private Transform trailL;
    private Transform trailR;
    private GameManager gameManager;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject rocket;
    private GameObject currentProjectile;
    private bool shootCooldown = false;
    private bool firestormShootCooldown = false;
    private bool hitCooldown = false;
    private bool isTutorial;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        SpawnGracePeriod();
        isTutorial = true;

        boostLightL = transform.Find("BoosterLightL");
        boostLightR = transform.Find("BoosterLightR");
        trailL = transform.Find("TrailL");
        trailR = transform.Find("TrailR");
        
    }
    private void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * thrust);
            
            BoosterEffects();
            
        }
        else
        {
            boostLightL.GetComponent<Light>().intensity = 3;
            boostLightR.GetComponent<Light>().intensity = 3;
            trailL.GetComponent<TrailRenderer>().emitting = false;
            trailR.GetComponent<TrailRenderer>().emitting = false;
        }

        
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(transform.forward * -thrust * 0.5f);
        }
        

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


        if (gameManager.isRocket)
        {
            currentProjectile = rocket;
        }
        else
        {
            currentProjectile = bullet;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTutorial)
            {
                gameManager.TutorialText.SetActive(false);
                gameManager.TutorialEndText.SetActive(true);
                StartCoroutine(GoodTime());
                isTutorial = false;
            }

            
            if (!shootCooldown)
             {
                 if (gameManager.spreadshot == Spreadshot.single)
                 {
                     Instantiate(currentProjectile, transform.position, transform.rotation);
                     StartCoroutine(CooldownTimer());

                  }
                 else if (gameManager.spreadshot == Spreadshot.twins)
                 {
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + 5, transform.rotation.z));
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y - 5, transform.rotation.z));
                     StartCoroutine(CooldownTimer());
                 }
                 else if (gameManager.spreadshot == Spreadshot.trishot)
                 {
                     Instantiate(currentProjectile, transform.position, transform.rotation);
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + 20, transform.rotation.z));
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y - 20, transform.rotation.z));
                     StartCoroutine(CooldownTimer());
                 }
                 else if (gameManager.spreadshot == Spreadshot.quad)
                 {
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + 5, transform.rotation.z));
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y - 5, transform.rotation.z));
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + 20, transform.rotation.z));
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y - 20, transform.rotation.z));
                     StartCoroutine(CooldownTimer());
                 }
                 else if (gameManager.spreadshot == Spreadshot.pentashot)
                 {
                     Instantiate(currentProjectile, transform.position, transform.rotation);
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + 15, transform.rotation.z));
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y - 15, transform.rotation.z));
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + 25, transform.rotation.z));
                     Instantiate(currentProjectile, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y - 25, transform.rotation.z));
                     StartCoroutine(CooldownTimer());
                 }
             }

             if (gameManager.isRocket)
             {
                 currentProjectile = rocket;
             }
             else
             {
                 currentProjectile = bullet;
             }
        }

        if (gameManager.isFirestorm)
        {
            if (!firestormShootCooldown)
            {
                Instantiate(bullet, transform.position, transform.rotation);
                for (int i = 1; i < 12; i++)
                {
                    Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + (15 * i), transform.rotation.z));
                    Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y - (15 * i), transform.rotation.z));
                }
                Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + 180, transform.rotation.z));
                StartCoroutine(FirestormCooldownTimer());
            }
        }

        if (transform.position.z < -13)
        {
            StartCoroutine(StopTrail());
            transform.position += new Vector3(0, 0, 24);
        }
        else if (transform.position.z > 13)
        {
            StartCoroutine(StopTrail());
            transform.position += new Vector3(0, 0, -24);
        }

        if (transform.position.x < -22)
        {
            StartCoroutine(StopTrail());
            transform.position += new Vector3(44, 0, 0);
        }
        else if (transform.position.x > 22)
        {
            StartCoroutine(StopTrail());
            transform.position += new Vector3(-44, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            if (!hitCooldown)
            {
                gameManager.lives--;
                if (gameManager.lives <= 0)
                {
                    Destroy(gameObject);
                }
                StartCoroutine(InvincibilityFrames());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickup pickup = other.GetComponent<Pickup>();

        if (pickup)
        {
            pickup.Activate();
        }
    }

    public void SpawnGracePeriod()
    {
        StartCoroutine(InvincibilityFramesSpawning());
    }

    private void BoosterEffects()
    {
        boostLightL.GetComponent<Light>().intensity = 10;
        boostLightR.GetComponent<Light>().intensity = 10;
        trailL.GetComponent<TrailRenderer>().emitting = true;
        trailR.GetComponent<TrailRenderer>().emitting = true;
    }
    IEnumerator CooldownTimer()
    {
        shootCooldown = true;
        yield return new WaitForSeconds(gameManager.bulletCooldown);
        shootCooldown = false;
    }
    IEnumerator FirestormCooldownTimer()
    {
        firestormShootCooldown = true;
        yield return new WaitForSeconds(0.1f);
        firestormShootCooldown = false;
    }

    IEnumerator InvincibilityFrames()
    {
        hitCooldown = true;
        for (int i = 0; i < 5; i++)
        {
            mr.forceRenderingOff = true;
            yield return new WaitForSeconds(0.1f);
            mr.forceRenderingOff = false;
            yield return new WaitForSeconds(0.1f);
        }
        hitCooldown = false;
    }

    IEnumerator InvincibilityFramesSpawning()
    {
        hitCooldown = true;
        for (int i = 0; i < 4; i++)
        {
            mr.forceRenderingOff = true;
            yield return new WaitForSeconds(0.2f);
            mr.forceRenderingOff = false;
            yield return new WaitForSeconds(0.2f);
        }
        hitCooldown = false;
    }

    IEnumerator StopTrail()
    {
        trailL.GetComponent<TrailRenderer>().enabled = false;
        trailR.GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        trailL.GetComponent<TrailRenderer>().enabled = true;
        trailR.GetComponent<TrailRenderer>().enabled = true;
    }

    IEnumerator GoodTime()
    {
        yield return new WaitForSeconds(0.75f);
        gameManager.TutorialEndText.SetActive(false);
    }

}
