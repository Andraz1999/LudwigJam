using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    private Rigidbody2D rb;

    public bool facingRight = true; 

    [Header("Movementvariables")]
    bool canMove = true;
    [SerializeField] private float movementAcceleration = 50f;
    [SerializeField] private float airMovementAcceleration = 10;
    [SerializeField] private float maxMoveSpeed = 10f;
    [SerializeField] private float linearDrag = 7f;
    private float horizontalDirection;
    private bool changingDirection => (rb.velocity.x > 0f && horizontalDirection < 0f ) || (rb.velocity.x < 0f && horizontalDirection > 0f);
    
    

    // gravity
    
    //                                float groundedGravity = -0.05f;
    // jumping variables
    [Header("Jumping variables")]
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float timeToApex = 0.5f;

    bool isJumpPressed = false;
    private float initialJumpVelocity;
    private float multiJumpVelocity;

    private float gravity = 1;

    [SerializeField] private float fallJumpMultiplier = 8f;
    [SerializeField] private float lowJumpMultiplier = 5f;

    [SerializeField] private float multiJumpMultiplier = 1f;
    [SerializeField] private int extraJumps;
    private int currentJump;

    [SerializeField] private float hangTime = 0.2f;
    private float hangCounter;
    [SerializeField] private float jumpBufferLength = 0.1f;
    private float jumpBufferCount;
    private float jumpDelay;
    private float jumpDelayCount;

    private float airLinearDrag = 1f;
    ///////////
    [Header("WallSliding")]
    [SerializeField] float wallSlideSpeed = 0;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float wallRaycastLength;
    private bool isTouchingWall;
    private bool wasTouchingWall;
    private bool isWallSliding;

    ///////////
    [Header("WallJumping")]
    [SerializeField] float wallJumpForce = 18f;
    int wallJumpDirection = -1;
    [SerializeField] Vector2 wallJumpAngle;
    
    ///////////
    [Header("Crouching")]
    [SerializeField] Vector3 crouchSize;
    private Vector3 basicSize;
    [SerializeField] float crouchSpeed;
    private float basicSpeed;
    private bool isCrouching;

    ///////////
    [Header("LayerMask")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private float groundRaycastOffSet;

    /////////////
    [Header("Particles")]
    [SerializeField] ParticleSystem footsteps;
    private ParticleSystem.EmissionModule footEmission;
    [SerializeField] float footestepsRateOverTime;
    [SerializeField] ParticleSystem impactEffect;
    private bool wasOnGround;
    private bool isGrounded;

    [Header("Switching Tabs")]
    [SerializeField] private Transform tab1;
    [SerializeField] private Transform tab2;
    private Vector3 tab1y;
    private Vector3 tab2y;
    private bool isSwitched;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetUpJumpVariables();
        jumpDelay = hangTime + 0.05f;
        footEmission = footsteps.emission;
        wallJumpAngle.Normalize();
        basicSize = transform.localScale;
        basicSpeed = maxMoveSpeed;

        tab1y = tab1.position;
        tab2y = tab2.position;
    }

        void Update()
    {
        if(canMove)
        MoveCharacter();

        if(rb.velocity.x < -0.01f && facingRight || rb.velocity.x > 0.01f && !facingRight)
        {
            Flip();
        }


        CheckCollisions();

        if(isGrounded && !isJumpPressed)
        {
            ApplyLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
        }


        if(isGrounded)
        {
            canMove = true;
            currentJump = extraJumps;
            hangCounter = hangTime;
            //ApplyLinearDrag();
            footEmission.rateOverDistance = footestepsRateOverTime;
            // dust Effect when hitting the ground
            if(!wasOnGround)
            {
                impactEffect.gameObject.SetActive(true);
                impactEffect.Stop();
                impactEffect.transform.position = footsteps.transform.position;
                impactEffect.Play();
            }
        }
        else
        {
            hangCounter -= Time.deltaTime;
           // ApplyAirLinearDrag();
            //FallMultiplier();
            footEmission.rateOverDistance = 0f;
        }

        
        

        if(jumpBufferCount > 0f)
        {
            CheckJump();
        }

        if(isTouchingWall && !wasTouchingWall) canMove = true;
        

        WallSlide();
        
        if(isGrounded && isCrouching)
        maxMoveSpeed = crouchSpeed;
        
        jumpBufferCount -= Time.deltaTime;
        jumpDelayCount -= Time.deltaTime; 
        wasOnGround = isGrounded;  
        wasTouchingWall = isTouchingWall;     
    }


    void SetUpJumpVariables()
    {
        currentJump = extraJumps;
        // float timeToApex = maxJumpTime/2;
        gravity = ((-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2)) / -9.81f;
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        // initialJumpVelocity = 9.81f * timeToApex;
        multiJumpVelocity = initialJumpVelocity * multiJumpMultiplier;

        rb.gravityScale = gravity;
        fallJumpMultiplier *= gravity;
        lowJumpMultiplier *= gravity;
    }

    public void Jump(float lowJump, float highJump)
    {
        if(jumpBufferCount > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, highJump);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, lowJump);
        }
    }

    public void Jump(float jump)
    {
        Jump(jump, jump);
    }

    private void CheckJump()
    {

        if(isWallSliding)
        {
            rb.AddForce(new Vector2(wallJumpForce*wallJumpDirection*wallJumpAngle.x, wallJumpForce*wallJumpAngle.y), ForceMode2D.Impulse);
            canMove = false;
        }

        else if(hangCounter > 0f)
        {
            // rb.velocity = new Vector2(rb.velocity.x, 0f);
            // float x = rb.velocity.x;
            // rb.velocity = new Vector2(x,initialJumpVelocity);
            Jump(initialJumpVelocity);
            canMove = true;
            
        }
        else if(currentJump > 0)
        {
            // rb.velocity = new Vector2(rb.velocity.x, 0f);
            // float x = rb.velocity.x;
            // rb.velocity = new Vector2(x,multiJumpVelocity);
            Jump(multiJumpVelocity);
            canMove = true;
            // rb.AddForce(Vector2.up*multiJumpVelocity, ForceMode2D.Impulse); 
            currentJump--; 
            
        }
        jumpBufferCount = 0f;
    }

    private void Flip()
    {
        wallJumpDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    // private void CancelJump()
    // {
    //     // if(rb.velocity.y > 0f)
    //     // {
    //     //     float x = rb.velocity.x;
    //     //     float y = rb.velocity.y / 2f;
    //     //     rb.velocity = new Vector2(x,y); 
    //     // }
    // }


    private void WallSlide()
    {
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            //wall slide
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    } 



    private void MoveCharacter()
    {
        if(isGrounded)
            rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration);
        else
            rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration / airMovementAcceleration);

        if(Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
    }

    private void ApplyLinearDrag()
    {
        // if(Mathf.Abs(horizontalDirection) < 0.4f || changingDirection)
        // {
            rb.drag = linearDrag;
        // }
        // else
        // {
        //     rb.drag = 0f;
        // }
    }

    private void ApplyAirLinearDrag()
    {
        rb.drag = airLinearDrag;
    }

    private void FallMultiplier()
    {
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = fallJumpMultiplier;
        }
        else if(rb.velocity.y > 0 && !isJumpPressed)
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    private void CheckCollisions()
    {
        isGrounded = Physics2D.Raycast(transform.position + Vector3.right*groundRaycastOffSet, Vector3.down, groundRaycastLength, groundLayer) || Physics2D.Raycast(transform.position - Vector3.right*groundRaycastOffSet, Vector3.down, groundRaycastLength, groundLayer);
        //if(isGrounded) hangCounter = hangTime;
        //isGrounded = isGrounded && !isJumpPressed;
        isTouchingWall = Physics2D.Raycast(transform.position, transform.right, wallRaycastLength, wallLayer);
    }

    private void SwitchTabs()
    {
        if(isSwitched)
        {
            tab1.position = tab1y;
            tab2.position = tab2y;
            isSwitched = false;
        }
        else
        {
            tab2.position = tab1y;
            tab1.position = tab2y;
            isSwitched = true;
        }
    }

    private void OnDrawGizmos()
    {
        // for ground Check
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.right*groundRaycastOffSet, transform.position + Vector3.right*groundRaycastOffSet + Vector3.down * groundRaycastLength);
        Gizmos.DrawLine(transform.position - Vector3.right*groundRaycastOffSet, transform.position - Vector3.right*groundRaycastOffSet + Vector3.down * groundRaycastLength);

        // for wall Check
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * wallRaycastLength);
        
    }

//////////////////////////////////////////INPUT//////////////////////////////////////////////

    public void MoveInput(InputAction.CallbackContext context)
    {
        if(context.performed)
            horizontalDirection = context.ReadValue<float>();
        if(context.canceled)
            horizontalDirection = 0f;
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if(context.started && jumpDelayCount < 0f)
        {
            //Jump();
            jumpDelayCount = jumpDelay;
            jumpBufferCount = jumpBufferLength;
            isJumpPressed = true;
        }
        else if(context.canceled)
        {
            //CancelJump();
            isJumpPressed = false;
        }
    }
    public void CrouchInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            transform.localScale = crouchSize;
            isCrouching = true;
        }
        if(context.canceled)
        {
            isCrouching = false;
            transform.localScale = basicSize;
            maxMoveSpeed = basicSpeed;
        }
    }

    public void SwitchInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SwitchTabs();
        }    
        
    }


}
