using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownJump : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerMovement player;
    [SerializeField] float jump;
    [SerializeField] JumpPlatform jumpPlatform;
    
    void Start()
    {
        player = PlayerMovement.Instance;
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            player.Jump(jump, jump);
            player.ResetDoubleJump();
            jumpPlatform.ForceBounce();
        }    
    }
}
