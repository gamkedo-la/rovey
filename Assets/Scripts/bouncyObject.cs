using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncyObject : MonoBehaviour
{
    public float bouncePower = 10.0f;
    public float wobbleScale = 1.0f;
    public float wobbleSpeed = 5.0f;
    public AudioSource bounceSound;
    public GameObject bounceSpawnPrefab;

    public void OnCollisionEnter (Collision collision)  {      
    
        Debug.Log("bouncyObject was colided with by " + collision.gameObject.name);
        
        //other.rigidbody.AddForce(Vector3.Up * force);

        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        if (bounceSound && collision.relativeVelocity.magnitude > 2)
            bounceSound.Play();
    }

}
