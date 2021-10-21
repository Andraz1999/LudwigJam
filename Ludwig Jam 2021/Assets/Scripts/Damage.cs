using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private PlayerStatus playerStatus;
    void Start()
    {
        playerStatus = PlayerStatus.Instance;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            playerStatus.Health(-10);
        }    
    }
}
