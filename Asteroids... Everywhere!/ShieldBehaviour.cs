using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    private GameManager gameManager;
    private bool invulnerable;
    private MeshRenderer meshRenderer;
    private void Start()
    {
        gameObject.SetActive(true);
        invulnerable = false;
        gameManager = FindObjectOfType<GameManager>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        transform.position = gameManager.Player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid" && !invulnerable)
        {
            Destroy(other.gameObject);
            if (ShieldPowerup.shieldLives >= 0)
            {
                ShieldPowerup.shieldLives--;
                StartCoroutine(ShieldHit());
            }
            else
            {
                gameManager.isShield = false;
            }
        }
    }

    private IEnumerator ShieldHit()
    {
        invulnerable = true;
        for (int i = 0; i < 5; i++)
        {
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        invulnerable = false;
    }
}
