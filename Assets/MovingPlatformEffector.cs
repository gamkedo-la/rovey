using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformEffector : MonoBehaviour
{
    public Vector3 effectiveVelocity = Vector3.zero;

    private Vector3 previousPosition;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        effectiveVelocity = (transform.position - previousPosition) - transform.up;
        previousPosition = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("ontriggerstay with player");
            var playerController = other.GetComponent<CharacterController>();
            if (playerController.isGrounded)
            {
                playerController.Move(effectiveVelocity);
            }
        }
    }
}
