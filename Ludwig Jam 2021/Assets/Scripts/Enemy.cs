using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;


    [SerializeField] private float walkSpeed = 10f;
    private bool facingRight = true; 

    // [SerializeField] bool allowToMove = true;
    // private float baseSpeed;

    [Header("Ground")]
    [SerializeField] float groundRaycastOffSet;
    [SerializeField] float groundRaycastLength;
    [SerializeField] LayerMask groundLayer;
    public bool isGrounded;

    [Header("Wall")]
    [SerializeField] float wallRaycastLength;
    [SerializeField] LayerMask wallLayer;
    bool isWall;

    [SerializeField] LayerMask borderLayer;
    bool isBorder;

    [SerializeField] float sightRange;
//////////////////////
    bool mustPatrol = true; 
    bool mustTurn;

    [SerializeField] Transform startPosition;

    //bool alreadyChecked;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>(); 
    //     baseSpeed = walkSpeed;  
    //     if(!allowToMove)
    //     {
    //         walkSpeed = 0;
    //     }
    }

    void Update()
    {   
        if((transform.position - player.position).magnitude < sightRange && !isGrounded)
        {
            rb.velocity = Vector2.zero;
            // if(!alreadyChecked)
            // {
            //     Invoke("Check", 3f);
            //     alreadyChecked = true;
            // }
            if(facingRight && (player.position.x - transform.position.x) < 0 || !facingRight && (player.position.x - transform.position.x) > 0)
            {
                Flip();
            }
        }
        else if(mustPatrol)
        {
            Patrol();
        }
        
    }
    // void Check()
    // {
    //     if(facingRight)
    // }

    private void FixedUpdate()
    {
        CheckCollisions();
        if(mustPatrol)
        {
            if((transform.position - player.position).magnitude < sightRange)
            {
                // walkSpeed = baseSpeed;
                mustTurn = (!isGrounded || isWall || Mathf.Sign(rb.velocity.x * (player.position.x - transform.position.x)) < 0);
            }
            else    mustTurn = (!isGrounded || isWall || isBorder);
        }

    }
    void CheckCollisions()
    {
        isGrounded = Physics2D.Raycast(transform.position + transform.right * groundRaycastOffSet, Vector3.down, groundRaycastLength, groundLayer);
        isWall = Physics2D.Raycast(transform.position, transform.right, wallRaycastLength, wallLayer);
        isBorder = Physics2D.Raycast(transform.position, transform.right, wallRaycastLength, borderLayer);
    }
    private void Patrol()
    {
        if(mustTurn)
        {
            Flip();
        }
        rb.velocity = Vector2.right * walkSpeed * Time.fixedDeltaTime + new Vector2(0f, rb.velocity.y);
    }

    private void Flip()
    {
        mustPatrol = false;
        facingRight = !facingRight;
        walkSpeed *= -1;
        // if((facingRight && walkSpeed < 0) || (!facingRight && walkSpeed > 0))
        // {
        //     walkSpeed *= -1;
        //     baseSpeed *= -1;
        // }
        transform.Rotate(0, 180, 0);
        mustPatrol = true;

    }

    public void Respawn()
    {
        transform.position = startPosition.position;
        // walkSpeed = Mathf.Abs(walkSpeed);
        // baseSpeed = Mathf.Abs(baseSpeed);
        if(!facingRight) Flip(); 
        // if(!allowToMove) walkSpeed = 0;
    }


    private void OnDrawGizmos()
    {
        // for player Check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // for ground Check
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.right * groundRaycastOffSet, transform.position + transform.right * groundRaycastOffSet + Vector3.down * groundRaycastLength);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * wallRaycastLength);
    }
    
}
