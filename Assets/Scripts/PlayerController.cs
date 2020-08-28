using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float turnSpeed = 6f;
    public float gravity = 2f;
    public float jumpStrength = 1f;

    private CharacterController controller;
    public Transform cam;
    public Animator animator;

    private Vector3 velocity;
    private float smoothTurnVelocity;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        transform.Rotate(transform.up, horizontal * turnSpeed * Time.deltaTime);

        var forward = Input.GetAxisRaw("Vertical");
        var inputDirection = new Vector3(0f, 0f, forward).normalized;
        var transformDirection = transform.TransformDirection(inputDirection);

        var flatMovement = speed * Time.deltaTime * transformDirection;
        velocity = new Vector3(flatMovement.x, velocity.y, flatMovement.z);

        if (PlayerJumped)
            velocity.y = jumpStrength;
        else if (controller.isGrounded)
            velocity.y = 0f;
        else
            velocity.y -= gravity * Time.deltaTime;

        if (velocity.z >= 0.1f || velocity.z <= -0.1f) animator.SetBool("Walking", true);
        controller.Move(velocity);
    }

    private bool PlayerJumped => controller.isGrounded && Input.GetAxisRaw("Jump") >= 0.1f;
}
