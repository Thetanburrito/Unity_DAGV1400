using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class PlayerController : MonoBehaviour
{
    // initializing variables needed for basic player movement.
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;


    public float groundDrag;


    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    
    public bool underneath;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    private int multiJumps;
    public int maxJumps;

    bool readyToJump;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public float slopeGravity;

    private bool exitingSlope;


    [Header("Misc")]
    public Transform orientation;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        multiJumps = maxJumps - 1;
        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        underneath = Physics.SphereCast(transform.position, 0.5f, Vector3.up, out RaycastHit hitinfo);


        MyInput();
        SpeedControl();
        StateHandler();

        // handeling  drag
        if (grounded)
        {
            rb.drag = groundDrag;
            multiJumps = maxJumps - 1;
        }
        else
            rb.drag = 0f;
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // movement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // jumping
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded && !Input.GetKey(crouchKey) && !underneath)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // midair jumping
        else if (Input.GetKeyDown(jumpKey) && readyToJump && multiJumps > 0 && !grounded)
        {
            multiJumps--;
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey) && ! underneath)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            state = MovementState.crouching;
        }
        // stop crouching
        if (!Input.GetKey(crouchKey) && ! underneath)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

    }

    private void StateHandler()
    {
        // mode - Sprinting
        if(grounded && Input.GetKey(sprintKey) && !Input.GetKey(crouchKey) && ! underneath)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // mode - Walking
        else if (grounded && !Input.GetKey(crouchKey) && ! underneath)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // mode - Crouching
        else if (grounded && Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
            
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else
        {
            // on ground
            if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            // in air
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        // turn off gravity while on slope
        rb.useGravity = !OnSlope();

    }

    private void Jump()
    {
        // this variable is needed to jump off slopes
        exitingSlope = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void SpeedControl()
    {


        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(slopeHit.normal * -slopeGravity, ForceMode.Acceleration);
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // not on slope
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }


    private void ResetJump()
    {
        exitingSlope = false;
        readyToJump = true;
    }

    private bool OnSlope()
    {
        if(Physics.SphereCast(transform.position, 0.5f, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.01f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }


    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
