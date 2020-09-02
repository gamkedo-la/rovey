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
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;


    // Start is called before the first frame update.
    void Start()
    {
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame.
    void FixedUpdate()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var forward = Input.GetAxisRaw("Vertical");
        var direction = new Vector3(horizontal, 0f, forward).normalized;
        velocity = new Vector3(0f, velocity.y, 0f);

        // Calculate movement direction.
        if (direction.sqrMagnitude >= 0.1f)
        {
            animator.SetBool("Walking", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            velocity += (speed * Time.deltaTime * (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward));
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        // Account for jumping.
        if (PlayerJumped)
            velocity.y = jumpStrength;
        else if (controller.isGrounded)
            velocity.y = 0f;
        else
            velocity.y -= gravity * Time.deltaTime;

        // Apply movement to character controller.
        controller.Move(velocity);
    }

    private bool PlayerJumped => controller.isGrounded && Input.GetAxisRaw("Jump") >= 0.1f;
}
