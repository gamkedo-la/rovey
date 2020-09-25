using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SproingEffector : MonoBehaviour
{
    public float sproingStrength = 2.0f;

    private Animator anim;
    private CharacterController targetCharacterController;
    private PlayerController targetPlayerController;
    private float gravity = 0f;
    private float velocity = 0f;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (targetPlayerController)
        {
            gravity += targetPlayerController.gravity * Time.fixedDeltaTime;
            velocity = sproingStrength - gravity;

            if (velocity > 0)
            {
                targetCharacterController.Move(transform.up * velocity);
            }
            else
            {
                EndSproing();
            }
        }
    }

    private void StartSproing()
    {
        targetPlayerController.sproinging = true;
        anim.SetTrigger("Sproing");
        gravity = 0f;
        velocity = 0f;
    }

    private void EndSproing()
    {
        targetPlayerController.sproinging = false;
        anim.ResetTrigger("Sproing");
        targetCharacterController = null;
        targetPlayerController = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var characterController = other.GetComponent<CharacterController>();
            if (characterController.isGrounded)
            {
                targetCharacterController = characterController;
                targetPlayerController = other.GetComponent<PlayerController>();
                StartSproing();
            }
        }
    }
}
