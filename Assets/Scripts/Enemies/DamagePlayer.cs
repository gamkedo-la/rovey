using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;



public class DamagePlayer : MonoBehaviour

{
    public float forceAngle;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
         
            bool isInvincible = other.gameObject.GetComponent<PlayerController>().isInvincible;
            ContactPoint contact = other.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;

            if (!isInvincible)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage();
                var targetAngle = Mathf.Atan2(position.x, position.z) * Mathf.Rad2Deg + rotation.eulerAngles.y;

                forceAngle = -targetAngle;

                // other.gameObject.GetComponent<PlayerController>().transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }
            else
            {
                return;
            }
        }
    }
}
