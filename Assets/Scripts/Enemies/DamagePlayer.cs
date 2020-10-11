using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;




public class DamagePlayer : MonoBehaviour
{ 

    public float knockback = 10f;
    private Vector3 knockVel = Vector3.zero;

    private CharacterController targetCharacterController;
    private PlayerController targetPlayerController;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             targetCharacterController = other.gameObject.GetComponent<CharacterController>();

            ContactPoint contact = other.contacts[0];
            Vector3 forceNormal = other.relativeVelocity;

            if (targetCharacterController)
            {

                targetPlayerController = other.gameObject.GetComponent<PlayerController>();

                bool isInvincible = targetPlayerController.isInvincible;

                if (!isInvincible)
                {
                    targetPlayerController.TakeDamage();
                    targetPlayerController.Shove(-forceNormal * knockback);
                }

                else
                {
                    return;
                }
            }
        }
    }
    
        
    

}
