using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawning : MonoBehaviour
{


    [SerializeField] private GameObject smallAS;
    [SerializeField] private GameObject mediumAS;
    [SerializeField] private GameObject largeAS;
    [SerializeField] private GameObject goldenAS;

    [SerializeField] private int minAsteroid;
    [SerializeField] private int maxAsteroid;
    [SerializeField] private int roundExtraMin;
    [SerializeField] private int roundExtraMax;
    private int RandomASAmount;
    public void SpawnAstroids(int roundNumber)
    {

        RandomASAmount = Random.Range(minAsteroid, maxAsteroid) + (roundNumber * Random.Range(roundExtraMin, roundExtraMax));

        for (int i = 0; i < RandomASAmount; i++)
        {
            float posX;
            float posY;
            float randomizerTypeAS = Random.Range(0, 100);
            posX = Random.Range(-20, 20);
            posY = Random.Range(-11, 11);

            transform.position = new Vector3(posX, 0, posY);

            if (randomizerTypeAS < 40)
            {
                Instantiate(mediumAS, transform.position, transform.rotation);
            }
            else if (randomizerTypeAS < 65)
            {
                Instantiate(largeAS, transform.position, transform.rotation);
            }
            else if (randomizerTypeAS < 90)
            {   
                Instantiate(smallAS, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(goldenAS, transform.position, transform.rotation);
            }
        }
    }
}
