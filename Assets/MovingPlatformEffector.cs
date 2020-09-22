using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformEffector : MonoBehaviour
{
    private Vector3 effectiveVelocity = Vector3.zero;
    private Vector3 previousPosition;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        var currentPosition = transform.position;
        effectiveVelocity = (currentPosition - previousPosition) - transform.up;
        previousPosition = currentPosition;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<CharacterController>();
            if (playerController.isGrounded)
            {
                playerController.Move(effectiveVelocity);
            }
        }
    }
}
