using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOnEnemy : MonoBehaviour
{
    SpriteRenderer sprite;
    [SerializeField]Collider2D col1;
    [SerializeField]Collider2D col2;
    [SerializeField] GameObject collectEffect;
    Enemy enemy;
    Rigidbody2D playerRB;
    PlayerMovement player;
    [SerializeField] float lowJump, highJump;
    

    [SerializeField] float respawnTime = 15f;

    [SerializeField] Transform noDoubleJump;
    [SerializeField] float offset;
    private void Start() 
    {
        sprite = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>(); 
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();   
        player = PlayerMovement.Instance;
    }

    void Update()
    {
        noDoubleJump.position = transform.position + Vector3.up*offset;
    }

    void Respawn()
    {
        sprite.enabled = true;
        col1.enabled = true;
        col2.enabled = true;
        enemy.Respawn();
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && playerRB.velocity.y < 0)
        {
            player.Jump(lowJump, highJump);
            player.ResetDoubleJump();
            sprite.enabled = false;
            col1.enabled = false;
            col2.enabled = false;
            Invoke("Respawn", respawnTime);
            Destroy(Instantiate(collectEffect, transform.position, transform.rotation), 5f);
        }    
    }
}
