using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            gameObject.SetActive(false);
        } else if (collision.gameObject.tag == "ghost") {
            collision.collider.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }  else if (collision.gameObject.tag == "projectile") {
            gameObject.SetActive(false);
        } 
    }
}
