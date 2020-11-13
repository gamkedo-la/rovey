using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float gravity = 2f;
    public float jumpStrength = 1f;
    public float jetpackStrength = 0.1f;
    public float terminalVelocity = -10.0f;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    private Vector3 knockVel = Vector3.zero;
    public PlayerStats playerStats;

    [Header("References and Events")]
    public Transform cam;
    public Animator animator;
    public UnityEvent playerDamaged; // pubsub event for player taking damage.
    private CharacterController controller;
    private Coroutine activeJetpackTimer;

    // FSM Variables
    [Header("Game State")]
    public bool sproinging = false; // Public variable hack for SproingPlatform.
    public bool knocked = false;
    public bool isInvincible = false;
    public bool startJump; // note: was private, made public for debug info
    public bool jetpacking;  // note: was private, made public for debug info
    public int KeyItems = 0;
    public float invincibilityDurationSeconds = 1f;
    public bool canJetpack = true;
    public bool queueJump = false;
    private Vector3 lastCheckpoint; // Position to reset the player after death.

    // spawn prefabs when things happen!
    [Header("Prefabs To Spawn When Stuff Happens")]
    public GameObject spawnOnJump;
    public GameObject spawnOnLand;
    public GameObject spawnOnJetpackStart;
    public GameObject spawnOnJetpackEnd;
    public GameObject spawnOnFootstep1;
    public GameObject spawnOnFootstep2;
    public GameObject spawnOnDamage;
    public GameObject spawnOnShove;
    public GameObject spawnOnBounce;
    public GameObject spawnOnPickup;
    public GameObject spawnOnCheckpoint;
    public GameObject spawnOnWin;
    public GameObject spawnOnGameover;

    // Steep slopes
    public float slideFriction = 0.3f;
    private Vector3 hitNormal;

    //floor trace
    private RaycastHit hit;
    private float dist;
    private Vector3 dir;

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

            if (!knocked)
            {
                
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                    turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                velocity += (speed * Time.deltaTime * (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward));
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        animator.SetBool("OnGround", controller.isGrounded);
        animator.SetBool("JumpButton", Input.GetButton("Jump"));

        if (controller.isGrounded)
        {
            var fallSpeed = velocity.y - (gravity * Time.deltaTime);
            velocity.y = -0.1f;
            playerStats.stamina = playerStats.maxStamina;

            if (queueJump)
            {

                animator.SetBool("JumpButton", true);
                animator.SetBool("QueueJump", true);

            }


        }
        else
        {
            var fallSpeed = velocity.y - (gravity * Time.deltaTime);
            velocity.y = Mathf.Clamp(fallSpeed, terminalVelocity, velocity.y);

            //floor trace

            dist = 5f;
            dir = new Vector3(0, -1, 0);
            
            if (Physics.Raycast(transform.position, dir, dist))
            {
                //the ray collided with something
                
                //disable jetpack
                canJetpack = false;
                animator.SetBool("CanJetpack", false);
                //check for and queue jump
                if (Input.GetButtonDown("Jump") )
                {
                    queueJump = true;
                }

            }
            else
            {
                //nothing was below your gameObject within dist.
                //enable jetpack
                canJetpack = true;
                animator.SetBool("CanJetpack", true);
            }

        }

        if (startJump)
        {
            startJump = false;
            velocity.y = jumpStrength;

            if (queueJump)
            {
                queueJump = false;
            }

        }

        if (jetpacking)
        {
            velocity.y = jetpackStrength;
            playerStats.stamina -= Time.deltaTime;

            if (!Input.GetButton("Jump") )
            { 
                StopJetpack();
                
            }
            if (playerStats.stamina <= 0f)
            {
                StopJetpack();
            }
        }

        if (sproinging)
        {
            velocity.y = 0;
        }

        if (knocked = true)
        {
            controller.Move(knockVel * Time.deltaTime);
            knockVel *= 0.9f;
            

            if( knockVel.x < 0.25 &&  knockVel.z < 0.25)
            { 
                knocked = false;
                animator.SetBool("Stagger", false);
            }
        }

        // Check for steep slopes.
        var isSliding = (Vector3.Angle(Vector3.up, hitNormal) >= controller.slopeLimit);
        if (isSliding)
        {
            velocity.x += (1f - hitNormal.y) * hitNormal.x * (1f - slideFriction);
            velocity.z += (1f - hitNormal.y) * hitNormal.z * (1f - slideFriction);
        }

        // Apply movement to character controller.
        controller.Move(velocity);
        checkWin();
    }

    public void StartJump()
    {
        startJump = true;
        if (spawnOnJump) Instantiate(spawnOnJump, transform.position, transform.rotation);

    }

    public void UnqueueJump()
    {
        animator.SetBool("QueueJump", false);
        animator.SetBool("JumpButton", false);
    }

    public void StartJetpack()
    {

        if (canJetpack)
        {
            jetpacking = true;
            activeJetpackTimer = StartCoroutine(JetpackTimer());
            animator.ResetTrigger("StopJetpack");
            if (spawnOnJetpackStart) Instantiate(spawnOnJetpackStart, transform.position, transform.rotation);
                spawnOnJetpackStart.transform.parent = (controller.transform);
        }
    }

    public void StopJetpack()
    {
        jetpacking = false;
        StopCoroutine(activeJetpackTimer);
        animator.SetTrigger("StopJetpack");
        if (spawnOnJetpackEnd) Instantiate(spawnOnJetpackEnd, transform.position, transform.rotation);

    }

    public void TakeDamage()
    {
        if (!isInvincible)
        {
            playerDamaged.Invoke();
            StartCoroutine(BecomeTemporarilyInvincible());
            knocked = true;
            animator.SetBool("Stagger", true);
            if (spawnOnDamage) Instantiate(spawnOnDamage, transform.position, transform.rotation);

        }
        else
        {
            return;
        }

    }

    public void Shove(Vector3 knockForce)
    {
        knockVel += knockForce;
        if (spawnOnShove) Instantiate(spawnOnShove, transform.position, transform.rotation);

    }

    public IEnumerator BecomeTemporarilyInvincible()
    {
        
        isInvincible = true;
        
        yield return new WaitForSeconds(invincibilityDurationSeconds);

        isInvincible = false;
      
    }
    
    private void checkWin()
    {
        if(KeyItems >= 3)
        {
            // insert level complete code
            if (spawnOnWin) Instantiate(spawnOnWin, transform.position, transform.rotation);

        }
    }

    private IEnumerator JetpackTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(playerStats.stamina);
            StopJetpack();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

        if (hit.gameObject.CompareTag("OutOfBounds"))
        {
            // @TODO some kinda death/falling animation before we reset position
            ResetPosition(lastCheckpoint);
        }
        else if (hit.gameObject.CompareTag("Checkpoint"))
        {
            lastCheckpoint = hit.transform.position;
            Debug.Log("checkpoint updated");
            if (spawnOnCheckpoint) Instantiate(spawnOnCheckpoint, transform.position, transform.rotation);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpoint = other.transform.position;
            Debug.Log("checkpoint updated");
            if (spawnOnCheckpoint) Instantiate(spawnOnCheckpoint, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("KeyItem"))
        {
            KeyItems++;
            if (spawnOnPickup) Instantiate(spawnOnPickup, transform.position, transform.rotation);
        }
    }

    private void ResetPosition(Vector3 targetPosition)
    {
        velocity = Vector3.zero;
        transform.position = targetPosition + (Vector3.up * controller.height/2);
    }

    private bool PlayerJumped => controller.isGrounded && !animator.GetBool("Jumping") && Input.GetAxisRaw("Jump") >= 0.1f;

}
