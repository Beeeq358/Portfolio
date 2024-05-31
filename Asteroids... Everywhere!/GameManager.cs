using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public enum Spreadshot
{
    single,
    twins,
    trishot,
    quad,
    pentashot
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject AsteroidSpawner;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI LivesText;
    public GameObject TutorialText;
    public GameObject TutorialEndText;
    public GameObject Player;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject FireStormPickup;
    [SerializeField] private GameObject ScorePickup;
    [SerializeField] private GameObject Shield;
    private Camera cam;
    public GameObject powerupButton1;
    public GameObject powerupButton2;
    public float bulletSpeed;
    public float destroyTimer;
    public float bulletCooldown;
    public Spreadshot spreadshot;
    private GameObject asteroidCheck;
    public static int score;
    public int lives;
    [SerializeField] private int startLives;
    public int currentRound;
    private bool roundOver;
    public bool isFirestorm;
    public bool isRocket;
    public bool isShield;
    private int spawnChance;

    public Powerup[] _powerups;
    private void Start()
    {
        cam = Camera.main;
        cam.backgroundColor = new Color(Random.Range(0f, 0.2f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
        endScreen.SetActive(false);
        TutorialText.SetActive(true);
        TutorialEndText.SetActive(false);
        powerupButton1.SetActive(false);
        powerupButton2.SetActive(false);
        AsteroidSpawner.GetComponent<AsteroidSpawning>().SpawnAstroids(currentRound);
        lives = startLives;
        score = 0;
        spreadshot = Spreadshot.single;
        bulletSpeed = 20;
        destroyTimer = 0.75f;
        bulletCooldown = 0.75f;
        currentRound = 0;
        isFirestorm = false;
        roundOver = false;
        isRocket = false;
        isShield = false;
    }

    private void Update()
    {
        asteroidCheck = GameObject.FindWithTag("Asteroid");

        if (asteroidCheck == null && !roundOver)
        {
            ChoosePowerup();
            roundOver = true;
        }

        if (lives <= 0)
        {
            endScreen.SetActive(true);
        }


        ScoreText.text = "Score: " + score.ToString();
        LivesText.text = "Lives: " + lives.ToString();

        Shield.SetActive(isShield);
        if (ShieldPowerup.shieldLives < 0 && !isShield)
        {
            ShieldPowerup.shieldLives = 2;
        }
    }

    private void ChoosePowerup()
    {
        Powerup powerupSlot1 = _powerups[Random.Range(0, _powerups.Length)];
        while ((powerupSlot1 == _powerups[1] && spreadshot == Spreadshot.pentashot) || (powerupSlot1 == _powerups[2] && destroyTimer == 0) || (powerupSlot1 == _powerups[4] && isRocket))
        {
            powerupSlot1 = _powerups[Random.Range(0, _powerups.Length)];
        }

        Powerup powerupSlot2 = _powerups[Random.Range(0, _powerups.Length)];
        while ((powerupSlot1 == powerupSlot2) || (powerupSlot2 == _powerups[1] && spreadshot == Spreadshot.pentashot) || (powerupSlot2 == _powerups[2] && destroyTimer == 0) || (powerupSlot2 == _powerups[4] && isRocket))
        {
            powerupSlot2 = _powerups[Random.Range(0, _powerups.Length)];
        }

        powerupButton1.GetComponentInChildren<TextMeshProUGUI>().text = powerupSlot1.displayName;
        powerupButton2.GetComponentInChildren<TextMeshProUGUI>().text = powerupSlot2.displayName;

        powerupButton1.GetComponent<Button>().onClick.RemoveAllListeners();
        powerupButton2.GetComponent<Button>().onClick.RemoveAllListeners();

        powerupButton1.GetComponent<Button>().onClick.AddListener(powerupSlot1.Apply);
        powerupButton2.GetComponent<Button>().onClick.AddListener(powerupSlot2.Apply);
        
        powerupButton1.SetActive(true);
        powerupButton2.SetActive(true);
    }
    public void NextRound()
    {
        cam.backgroundColor = new Color(Random.Range(0f, 0.2f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
        currentRound++;
        AsteroidSpawner.GetComponent<AsteroidSpawning>().SpawnAstroids(currentRound);
        roundOver = false;
        Player.GetComponent<PlayerMovement>().SpawnGracePeriod();
        spawnChance = Random.Range(0, 70);

        float wait = Random.Range(5, 15);
        Debug.Log("wait " + wait);
        Debug.Log("spawnChance " + spawnChance);
        while (wait > 0)
        {
            wait = wait - Time.deltaTime;
        }
        if (wait < 0)
        {
            float posX;
            float posY;
            if (spawnChance >= 60)
            {
                posX = Random.Range(-20, 20);
                posY = Random.Range(-11, 11);
                Instantiate(FireStormPickup, new Vector3(posX, 0, posY), transform.rotation);

            }

            if (spawnChance <= 25)
            {

                posX = Random.Range(-20, 20);
                posY = Random.Range(-11, 11);
                Instantiate(ScorePickup, new Vector3(posX, 0, posY), transform.rotation);
            }
        }
    }

    public void StartFireStorm()
    {
        StartCoroutine(FireStormLifespan());
    }

    public IEnumerator FireStormLifespan()
    {
        isFirestorm = true;
        yield return new WaitForSeconds(0.5f);
        isFirestorm = false;
    }
}
