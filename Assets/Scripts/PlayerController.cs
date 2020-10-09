using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = 2f;
    public float jumpStrength = 1f;
    public float jetpackStrength = 0.1f;
    public float maxJetpackTime = 2.0f;
    public float terminalVelocity = -10.0f;
    public int collectedCollectables = 0;

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

    // Position to reset the player after death.
    private Vector3 lastCheckpoint;

    // pubsub event for player taking damage.
    public UnityEvent playerDamaged;
    public bool isInvincible = false;
    public float invincibilityDurationSeconds = 1f;

    public Vector3 LastCheckpoint
    {
        get => lastCheckpoint;
        set => lastCheckpoint = value;
    }

    private void Awake()
    {
        lastCheckpoint = transform.position;
    }

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
        checkWin();
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

    public void TakeDamage()
    {
        if (!isInvincible)
        {
            playerDamaged.Invoke();
            StartCoroutine(BecomeTemporarilyInvincible());
        }
        else
        {
            return;
        }

    }

    public IEnumerator BecomeTemporarilyInvincible()
    {
        //Debug.Log("Player turned invincible!");
        isInvincible = true;
        animator.SetBool("Invincible", true);

        yield return new WaitForSeconds(invincibilityDurationSeconds);

        isInvincible = false;
        animator.SetBool("Invincible", false);
        //Debug.Log("Player is no longer invincible!");
    }
    
    private void checkWin()
    {
        if(collectedCollectables >= 3)
        {
            // insert level complete code
        }
    }

    private IEnumerator JetpackTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(maxJetpackTime);
            StopJetpack();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("OutOfBounds"))
        {
            // @TODO some kinda death/falling animation before we reset position
            ResetPosition(lastCheckpoint);
        }
        else if (hit.gameObject.CompareTag("Checkpoint"))
        {
            lastCheckpoint = hit.transform.position;
            Debug.Log("checkpoint updated");
        }
        else if (hit.gameObject.CompareTag("Collectable"))
        {
            collectedCollectables++;
            Destroy(hit.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpoint = other.transform.position;
            Debug.Log("checkpoint updated");
        }
    }

    private void ResetPosition(Vector3 targetPosition)
    {
        velocity = Vector3.zero;
        transform.position = targetPosition + (Vector3.up * controller.height/2);
    }

    private bool PlayerJumped => controller.isGrounded && !animator.GetBool("Jumping") && Input.GetAxisRaw("Jump") >= 0.1f;

}
