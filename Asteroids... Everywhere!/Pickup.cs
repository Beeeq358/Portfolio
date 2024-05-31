using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    protected GameManager game;
    [SerializeField] protected int liveTime;

    private void Awake()
    {
        game = FindObjectOfType<GameManager>();
        StartCoroutine(LiveSpan());
    }
    public virtual void Activate()
    {
        Destroy(gameObject);
    }

    IEnumerator LiveSpan()
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(gameObject);
    }
}
