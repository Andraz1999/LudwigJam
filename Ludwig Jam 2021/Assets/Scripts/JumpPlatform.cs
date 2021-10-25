using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] float lowJump, highJump;
    [SerializeField] Transform noDoubleJump;
    Animator animator;
    [SerializeField] float offset;
    public float bonus = 0.5f;
    
    void Start()
    {
        player = PlayerMovement.Instance;
        animator = GetComponent<Animator>();
    }

   
    void Update()
    {
        noDoubleJump.position = transform.position + Vector3.up*offset;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            player.Jump(lowJump, highJump, bonus);
            player.ResetDoubleJump();
            animator.SetTrigger("jump");
        }    
    }
    public void ForceBounce()
    {
        animator.SetTrigger("jump");
    }

}
