using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Collectable : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent navMeshAgent;
    public PlayerMovement playerMovement;
    
    bool is_collected = false;
    
    void Start() {
        navMeshAgent.SetDestination (transform.position);
    }
    void OnTriggerEnter(Collider other) 
    {
        if (!is_collected && other.transform == player)
        {
            is_collected=true;
            playerMovement.collectables.Add(gameObject);
        }
    }

    void Update() {
        Vector3 forwardLook = playerMovement.m_Rotation * new Vector3(Random.Range(-1.0f,1.0f),0.0f,Random.Range(-1.0f,1.0f));
        if (is_collected) {
            navMeshAgent.SetDestination(player.position-forwardLook);
        }
    }
}


