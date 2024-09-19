using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AINavMesh : MonoBehaviour
{
    public enum Paths
    {
        ideal,
        middle,
        idealAlternate,
        middleAlternate
    }
    public Paths aipath;
    private NavMeshAgent agent;
    public List<Vector3> idealLines = new List<Vector3>();
    public List<Vector3> middleLines = new List<Vector3>();
    public List<Vector3> idealAlternateLines = new List<Vector3>();
    public List<Vector3> middleAlternateLines = new List<Vector3>();

    private List<Vector3> targetLines = new List<Vector3>();
    public Transform CPUObj;
    [SerializeField] private float slowDistance;
    private int targetedNode;
    private float aiDistance;
    public float checkDistance = 3;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChooseNewPath(false, (int)aipath);
    }

    void Update()
    {
        aiDistance = Vector3.Distance(transform.position, CPUObj.transform.position);

        if (aiDistance >= slowDistance)
        {
            if (agent.speed > 2)
            {
                agent.speed -= 0.5f;
            }
        }
        else
        {
            agent.speed = 60;
        }

        if (agent.remainingDistance < checkDistance)
        {
            targetedNode++;
            if (targetedNode == targetLines.Count)
            {
                targetedNode = 0;
            }
            agent.SetDestination(targetLines[targetedNode]);
        }
    }

    public void AIRespawn()
    {
        Vector3 closest = targetLines[targetedNode];
        for (int i = 0; i < targetLines.Count; i++)
        {
            if (Vector3.Distance(CPUObj.transform.position, closest) > Vector3.Distance(CPUObj.transform.position, targetLines[i]))
            {
                closest = targetLines[i];
                targetedNode = i + 1;
            }
        }
        transform.position = closest;
    }

    public void ChooseNewPath(bool random, int path)
    {
        if (random)
            aipath = (Paths)Random.Range(0, 4);
        else
            aipath = (Paths)path;

        Transform transformIdeal = GameObject.Find("Ideal Lines").transform;
        Transform transformMiddle = GameObject.Find("Middle Lines").transform;
        Transform transformAlternateIdeal = GameObject.Find("Ideal Alternate Lines").transform;
        Transform transformAlternateMiddle = GameObject.Find("Middle Alternate Lines").transform;
        Transform transform;

        switch (aipath)
        {
            case Paths.ideal:
                targetLines = idealLines;
                transform = transformIdeal;
                break;
            case Paths.idealAlternate:
                targetLines = idealAlternateLines;
                transform = transformAlternateIdeal;
                break;
            case Paths.middleAlternate:
                targetLines = middleAlternateLines;
                transform = transformAlternateMiddle;
                break;
            default:
                targetLines = middleLines;
                transform = transformMiddle;
                break;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            targetLines.Add(transform.GetChild(i).position);
        }
        targetedNode = 0;
        agent.SetDestination(targetLines[targetedNode]);
    }


}
