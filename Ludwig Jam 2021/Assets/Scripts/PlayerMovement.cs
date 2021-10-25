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
    [SerializeField] float wallRaycastOffset;
    [SerializeField] float wallRaycastLength;
    private bool isTouchingWall;
    private bool wasTouchingWall;
    private bool isWallSliding;

    ///////////
    [Header("WallJumping")]
    [SerializeField] float wallJumpForce = 18f;
    int wallJumpDirection = -1;
    [SerializeField] Vector2 wallJumpAngle;
    [SerializeField] float canMoveAgainDelay = 0.2f;
    
    ///////////
    [Header("Crouching")]
    [SerializeField] Vector2 crouchSize;
    [SerializeField] Vector2 crouchOffset;
    private CapsuleCollider2D col;
    private Vector2 startSize;
    private Vector2 startOffset;
    [SerializeField] float crouchSpeed;
    private float basicSpeed;
    private bool isCrouching;
    private bool crouchingPressed;
    private bool isCeiling;
    [SerializeField] float ceilingRaycastLength;

    ///////////
    [Header("LayerMask")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask noDoubleJumpLayer;
    [SerializeField] float noDoubleJumpRange;

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private float groundRaycastOffSet;

    /////////////
    [Header("Particles")]
    [SerializeField] ParticleSystem footsteps;
    private ParticleSystem.EmissionModule footEmission;
    [SerializeField] float footestepsRateOverTime;
    [SerializeField] ParticleSystem wallSlideEffect;
    private ParticleSystem.EmissionModule wallSlideEmission;
    [SerializeField] float wallSlideRateOverTime;
    [SerializeField] ParticleSystem impactEffect;
    [SerializeField] ParticleSystem wallJumpEffect;
    private bool wasOnGround;
    private bool isGrounded;

    [Header("Switching Tabs")]
    [SerializeField] private Transform tab1;
    [SerializeField] private Transform tab2;
    private Vector3 tab1y;
    private Vector3 tab2y;
    private bool isSwitched;

    [Header("Minimap Sprites")]
    [SerializeField] Transform minimapSprite;
    private float distanceBetweenTabs;


    [Header("Respawn")]
    //[SerializeField] Transform respawnPoint;
    Vector3 respawnPoint;
    [SerializeField] Animator animator;

    /////audio
    private PlayerAudio playerAudio;
    private AudioManager audioManager;

    ///////////////////////////////////////////////
    #region Singleton
    public static PlayerMovement Instance {get; private set;}
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }   
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetUpJumpVariables();
        jumpDelay = hangTime + 0.05f;
        footEmission = footsteps.emission;
        wallSlideEmission = wallSlideEffect.emission;
        wallJumpAngle.Normalize();
        col = GetComponent<CapsuleCollider2D>();
        startSize = col.size;
        startOffset = col.offset;
        //basicSize = transform.localScale;
        basicSpeed = maxMoveSpeed;

        tab1y = tab1.position;
        tab2y = tab2.position;
        distanceBetweenTabs = Mathf.Abs(tab1y.y - tab2y.y); 
        respawnPoint = transform.position;

        audioManager = AudioManager.Instance;

        playerAudio = PlayerAudio.Instance;
    }

        void FixedUpdate()
    {
        if(canMove)
        MoveCharacter();

        if(rb.velocity.x < -0.01f && facingRight || rb.velocity.x > 0.01f && !facingRight)
        {
            Flip();
        }


        CheckCollisions();
        animator.SetBool("isGrounded", isGrounded);

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
                JumpAudio();
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

        if(isTouchingWall && !wasTouchingWall)
        {   
            JumpAudio();
            wallJumpEffect.gameObject.SetActive(true);
            wallJumpEffect.Stop();
            wallJumpEffect.transform.position = transform.position + transform.right * 0.5f;
            wallJumpEffect.Play();
        }
        

        WallSlide();
        if(isWallSliding)
        {
            //wall slide
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
            wallSlideEmission.rateOverDistance = wallSlideRateOverTime;
        }
        else
        {
            wallSlideEmission.rateOverDistance = 0f;
        }
        animator.SetBool("isSliding", isWallSliding);
        
        if(isCrouching)
            {
                if(isGrounded)
                {
                    maxMoveSpeed = crouchSpeed;
                }
                if(!isCeiling && !crouchingPressed)
                StopCrouching();
            }
        animator.SetBool("isCrouching", isCrouching);
            

        
        
        jumpBufferCount -= Time.deltaTime;
        jumpDelayCount -= Time.deltaTime; 
        wasOnGround = isGrounded;  
        wasTouchingWall = isTouchingWall;   

        /// updating the minimap
        minimapSprite.position = new Vector3(transform.position.x, transform.position.y + distanceBetweenTabs, transform.position.z);  
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

    public void Jump(float lowJump, float highJump, float bonus = 0.3f)
    {   
        
        if(jumpBufferCount + bonus > 0f)
        {
            //Debug.Log(highJump);
            audioManager.PlayNotForced("highJump");
            rb.velocity = new Vector2(rb.velocity.x, highJump);
        }
        else
        {
            //Debug.Log(lowJump);
            audioManager.PlayNotForced("lowJump");
            rb.velocity = new Vector2(rb.velocity.x, lowJump);
        }
    }

    public void ResetDoubleJump()
    {
        currentJump = extraJumps;
    }
    public void Jump(float jump)
    {
        Jump(jump, jump);
    }

    private void CheckJump()
    {

        if(isWallSliding)
        {
            audioManager.PlayNotForced("jump");
            rb.AddForce(new Vector2(wallJumpForce*wallJumpDirection*wallJumpAngle.x, wallJumpForce*wallJumpAngle.y), ForceMode2D.Impulse);
            canMove = false;
            //ResetDoubleJump();// now you can double jump after wall jump
            Invoke("CanMoveAgain", canMoveAgainDelay);
        }

        else if(hangCounter > 0f)
        {
            audioManager.PlayNotForced("jump");
            rb.velocity = new Vector2(rb.velocity.x, initialJumpVelocity);
            //Jump(initialJumpVelocity);
            canMove = true;
            
        }
        else if(currentJump > 0)
        {   
            if(!Physics2D.OverlapCircle(transform.position, noDoubleJumpRange, noDoubleJumpLayer))
            {
                animator.SetTrigger("DoubleJump");
                audioManager.PlayNotForced("jump");
                
                rb.velocity = new Vector2(rb.velocity.x, multiJumpVelocity);
                //Jump(multiJumpVelocity);
                canMove = true;
                // rb.AddForce(Vector2.up*multiJumpVelocity, ForceMode2D.Impulse); 
                currentJump--; 
            }
            
        }
        jumpBufferCount = 0f;
    }

    private void CanMoveAgain()
    {
        if(!isGrounded)
        canMove = true;
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
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0 && !isCrouching)
        {
            isWallSliding = true;
            WallSlidingAudio();
            canMove = false;
            //wall slide
            //rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
            WallSlidingAudioStop();
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

    private void StopCrouching()
    {
        isCrouching = false;
        //transform.localScale = basicSize;
        col.size = startSize;
        col.offset = startOffset;
        maxMoveSpeed = basicSpeed;
    }

    private void CheckCollisions()
    {
        isGrounded = Physics2D.Raycast(transform.position + Vector3.right*groundRaycastOffSet, Vector3.down, groundRaycastLength, groundLayer) || Physics2D.Raycast(transform.position - Vector3.right*groundRaycastOffSet, Vector3.down, groundRaycastLength, groundLayer);
        //if(isGrounded) hangCounter = hangTime;
        //isGrounded = isGrounded && !isJumpPressed;
        isTouchingWall = Physics2D.Raycast(transform.position + Vector3.up * wallRaycastOffset, transform.right, wallRaycastLength, wallLayer);
        isCeiling = Physics2D.Raycast(transform.position, transform.up, ceilingRaycastLength, groundLayer);
    }

    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        transform.position = respawnPoint;
        if(isSwitched)
        SwitchTabs();
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
        Gizmos.DrawLine(transform.position + Vector3.up * wallRaycastOffset, transform.position + Vector3.up * wallRaycastOffset + transform.right * wallRaycastLength);

        // for ceiling Check
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * ceilingRaycastLength);

        // for double jumps
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, noDoubleJumpRange);
        
    }

//////////////////////////////////////////INPUT//////////////////////////////////////////////

    public void MoveInput(InputAction.CallbackContext context)
    {
        if(context.performed)
            horizontalDirection = context.ReadValue<float>();

        if(context.canceled)
            horizontalDirection = 0f;
        animator.SetFloat("InputStrength", Mathf.Abs(horizontalDirection));
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
            //transform.localScale = crouchSize;
            col.size = crouchSize;
            col.offset = crouchOffset;
            crouchingPressed = true;
            isCrouching = true;
            if(isWallSliding)
            {
                canMove = true;
                isWallSliding = false;
                WallSlidingAudioStop();
            }
        }
        if(context.canceled)
        {
            crouchingPressed = false;
        }
    }

    public void SwitchInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SwitchTabs();
        }    
        
    }
    
////////////////////// AUDIO
    
    public bool CheckIfSwitched()
    {
        return isSwitched;
    }
    private void JumpAudio()
    {   
        if(isSwitched)
            {
                //audioManager.RandomPlay("jumpTrava", 1f, 1f, 0.8f, 1.1f);
                audioManager.Play("jumpTrava");
            }
        else
        {
            //audioManager.RandomPlay("jumpBeton", 1f, 1f, 0.8f, 1.1f);
            audioManager.Play("jumpBeton");
        }
    }
    private void WallSlidingAudio()
    {
        audioManager.PlayNotForced("slide");
    }
    private void WallSlidingAudioStop()
    {
        audioManager.StopPlaying("slide");
    }
}
