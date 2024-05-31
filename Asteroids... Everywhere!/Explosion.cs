using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem PSsmall;
    private ParticleSystem PSbig;
    private Vector3 bulletPosition;
    private void Start()
    {
        GameObject bulletGO = GameObject.Find("Bullet(Clone)");
        if (bulletGO == null)
        {
            bulletGO = GameObject.Find("Rocket(Clone)");
        }
        bulletPosition = bulletGO.transform.position;
        if (gameObject.name == "SmallExplosion(Clone)")
        {
            PSsmall = GetComponentInChildren<ParticleSystem>();
            //Debug.Log("PSsmall assigned: " + (PSsmall != null));
        }
        else if (gameObject.name == "BigExplosion(Clone)")
        {
            PSbig = GetComponentInChildren<ParticleSystem>();
            //Debug.Log("PSbig assigned: " + (PSbig != null));
        }
        gameObject.transform.LookAt(bulletPosition);
        StartCoroutine(PlayExplosion());
    }


    IEnumerator PlayExplosion()
    {
        float delay;
        if (PSsmall != null)
        {
            PSsmall.Play();
            delay = 0.1f;
        }
        else if (PSbig != null)
        {
            PSbig.Play();
            delay = 0.2f;
        }
        else
        {
            Debug.LogError("no PSsmall or PSbig component detected");
            delay = 0f;
        }
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
