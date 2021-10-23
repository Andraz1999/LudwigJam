using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform player;

    // Patroling
    [SerializeField] Vector3 walkPoint1;
    [SerializeField] Vector3 walkPoint2;
    private bool isSecond;
    private Vector3 walkPoint;
    bool walkPointSet;

    [SerializeField] float sightRange;


    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();   
    }

    void Update()
    {
        if ((transform.position - player.position).magnitude < sightRange)
        ChasePlayer();
        else
        Patroling();
    }
    private void Patroling()
    {
        if(!walkPointSet) SearchWalkPoint();
        else agent.SetDestination(walkPoint);

        if((transform.position - walkPoint).magnitude < 1f)
        walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        if(isSecond)
        {
            walkPoint = walkPoint1;
            isSecond = false;
        }
        
        else
        {
            walkPoint = walkPoint2;
            isSecond = true;
        }
        walkPointSet = true;


    } 


    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    
}
