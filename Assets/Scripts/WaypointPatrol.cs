using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public float wanderRadius;
    public NavMeshAgent navMeshAgent;
    // public Transform[] waypoints;
    public Transform player; 
    public Transform exclaim;

    int m_CurrentWaypointIndex;
    public bool m_Detected;
    void Start ()
    {
        // navMeshAgent.SetDestination (waypoints[0].position);
        exclaim.gameObject.SetActive(false);
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_Detected = true;
            exclaim.gameObject.SetActive(true);
            navMeshAgent.speed *= 1.5f;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_Detected = false;
            exclaim.gameObject.SetActive(false);
            navMeshAgent.speed /= 1.5f;
        }
    }

    void Update ()
    {
        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            Vector3 newPos;
            if (m_Detected) {
                newPos = player.position;
            } else {
                newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            }
            navMeshAgent.SetDestination (newPos);
            
        }
    }

    public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           
            randomDirection += origin;
           
            NavMeshHit navHit;
           
            NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
           
            return navHit.position;
        }

}
