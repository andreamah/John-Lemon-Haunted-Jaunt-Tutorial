using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public float wanderRadius;
    public NavMeshAgent navMeshAgent;
    public Transform player; 
    public Transform exclaim;
    public Transform projectile; 
    public Collider selfCollider;
    public GameEnding gameEnding;
    public float ghostMinSpeed = 1.4f;
    public float ghostMaxSpeed = 2.4f;
    int m_CurrentWaypointIndex;
    bool m_Detected;
    bool playerCaught = false;
    public bool chasing = false;

    void Start ()
    {
        exclaim.gameObject.SetActive(false);
        
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        navMeshAgent.SetDestination(newPos);
    }

    // if the player collides with the ghost, trigger the end screen, even
    // if the player if out of the ghost's line of sight
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.thisCollider == selfCollider && contact.otherCollider.transform == player){
                playerCaught = true;
            }
        }
    }
    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_Detected = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_Detected = false;
        }
    }

    void Update ()
    {
        if (playerCaught) {
            gameEnding.CaughtPlayer ();
        } else {
            bool donePath = (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance);
            // if you've been detected, only
            if (m_Detected) {
                RaycastHit raycastHit;
                Vector3 direction = player.position - transform.position;
                Ray ray = new Ray(transform.position, direction);
                
                bool new_chasing;
                if(Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.transform == player))
                {
                    new_chasing = true;
                } else {
                    // in range, but there is a wall separating
                    new_chasing = false;
                }
                
                if ((new_chasing && !chasing) || donePath) {
                    //either done the current path or going from not chasing to to chasing
                    chasing = new_chasing;
                    UpdateGamePos();
                }
                chasing = new_chasing;

            } else if(donePath)
            {
                // if nothing is detected, just proceed normally
                chasing = false;
                UpdateGamePos();
                
            }
        }
    }

    void UpdateGamePos() {
        Vector3 newPos;
        if (chasing) {
            exclaim.gameObject.SetActive(true);
            navMeshAgent.speed = ghostMaxSpeed;
            newPos = player.position;
        } else {
            exclaim.gameObject.SetActive(false);
            navMeshAgent.speed = ghostMinSpeed;
            newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        }
        navMeshAgent.SetDestination(newPos);
    }

    public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        
        randomDirection += origin;
        
        NavMeshHit navHit;
        
        NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
        
        return navHit.position;
    }

        
    
}
