using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    private Rigidbody2D rb;

    [Header("Movementvariables")]
    [SerializeField] private float movementAcceleration = 50f;
    [SerializeField] private float airMovementAcceleration = 10;
    [SerializeField] private float maxMoveSpeed = 10f;
    [SerializeField] private float linearDrag = 7f;
    private float horizontalDirection;
    private bool changingDirection => (rb.velocity.x > 0f && horizontalDirection < 0f ) || (rb.velocity.x < 0f && horizontalDirection > 0f);
    
    

    // gravity
    
    float groundedGravity = -0.05f;
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

    private float airLinearDrag = 1f;

    [Header("LayerMask")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private float groundRaycastOffSet;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetUpJumpVariables();
        
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

    

    private void Jump()
    {

        if(hangCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            float x = rb.velocity.x;
            rb.velocity = new Vector2(x,initialJumpVelocity);
            jumpBufferCount = 0f;
        }
        else if(currentJump > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            float x = rb.velocity.x;
            rb.velocity = new Vector2(x,multiJumpVelocity);
            // rb.AddForce(Vector2.up*multiJumpVelocity, ForceMode2D.Impulse); 
            currentJump--; 
            jumpBufferCount = 0f;
        }
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


    void Update()
    {
        MoveCharacter();

        CheckCollisions();
        if(isGrounded)
        {
            currentJump = extraJumps;
            hangCounter = hangTime;
            ApplyLinearDrag();
        }
        else
        {
            hangCounter -= Time.deltaTime;
            ApplyAirLinearDrag();
            FallMultiplier();
        }
        if(jumpBufferCount > 0f)
        {
            Jump();
        }
        
        jumpBufferCount -= Time.deltaTime;
        
            
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
        if(isGrounded) hangCounter = hangTime;
        isGrounded = isGrounded && !isJumpPressed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.right*groundRaycastOffSet, transform.position + Vector3.right*groundRaycastOffSet + Vector3.down * groundRaycastLength);
        Gizmos.DrawLine(transform.position - Vector3.right*groundRaycastOffSet, transform.position - Vector3.right*groundRaycastOffSet + Vector3.down * groundRaycastLength);
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
        if(context.started)
        {
            //Jump();
            jumpBufferCount = jumpBufferLength;
            Debug.Log(jumpBufferCount);
            isJumpPressed = true;
        }
        else if(context.canceled)
        {
            //CancelJump();
            isJumpPressed = false;
        }
    }
}
