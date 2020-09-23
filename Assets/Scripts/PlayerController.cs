using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = 2f;
    public float jumpStrength = 1f;
    public float jetpackStrength = 0.1f;
    public float maxJetpackTime = 2.0f;
    public float terminalVelocity = -10.0f;

    private CharacterController controller;

    public Transform cam;
    public Animator animator;

    private Vector3 velocity;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    // FSM Variables
    private bool startJump;
    private bool jetpacking;
    private Coroutine activeJetpackTimer;

    // Public variable hack for SproingPlatform.
    public bool sproinging = false;

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
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            velocity += (speed * Time.deltaTime * (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward));
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        animator.SetBool("OnGround", controller.isGrounded);
        animator.SetBool("JumpButton", Input.GetButton("Jump"));

        if (controller.isGrounded)
        {
            velocity.y = -0.1f;
        }
        else
        {
            var fallSpeed = velocity.y - (gravity * Time.deltaTime);
            velocity.y = Mathf.Clamp(fallSpeed, terminalVelocity, velocity.y);
        }

        if (startJump)
        {
            startJump = false;
            velocity.y = jumpStrength;
        }

        if (jetpacking)
        {
            velocity.y = jetpackStrength;

            if (!Input.GetButton("Jump"))
            {
                StopJetpack();
            }
        }

        if (sproinging)
        {
            velocity.y = 0;
        }

        // Apply movement to character controller.
        controller.Move(velocity);
    }

    public void StartJump()
    {
        startJump = true;
    }

    public void StartJetpack()
    {
        jetpacking = true;
        activeJetpackTimer = StartCoroutine(JetpackTimer());
        animator.ResetTrigger("StopJetpack");
    }

    public void StopJetpack()
    {
        jetpacking = false;
        StopCoroutine(activeJetpackTimer);
        animator.SetTrigger("StopJetpack");
    }

    IEnumerator JetpackTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(maxJetpackTime);
            StopJetpack();
        }
    }

    private bool PlayerJumped => controller.isGrounded && !animator.GetBool("Jumping") && Input.GetAxisRaw("Jump") >= 0.1f;

}
