using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;
    Vector3 m_Movement;
    public float turnSpeed =20f;
    Rigidbody m_Rigidbody;
    public Quaternion m_Rotation = Quaternion.identity;
    // public Projectile proj;
    public Rigidbody projectile;
    public AudioSource m_AudioSource;
    public List<GameObject> collectables = new List<GameObject>();
    public int projectileMag = 500;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);

        
        if (Input.GetMouseButtonDown(0))
            shootProjectile();

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
          
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }

    }

    void shootProjectile() {
        if (collectables.Count>0) {
            Vector3 forwardLook = m_Rotation * new Vector3(0.0f,0.0f,1);
            // instantiate it slightly forward of the player's position and upwards a unit
            Rigidbody proj = Instantiate(projectile, transform.position+(forwardLook*0.5f)+Vector3.up, Quaternion.identity);
            proj.AddForce(forwardLook * projectileMag);
            collectables[0].SetActive(false);
            collectables.RemoveAt(0);
        }

    }
    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude*2.0f);
        m_Rigidbody.MoveRotation (m_Rotation);
    }
}
