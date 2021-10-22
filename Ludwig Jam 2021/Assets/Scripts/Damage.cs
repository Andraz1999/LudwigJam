using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private PlayerStatus playerStatus;
    [SerializeField] int damage = -10;
    [SerializeField] bool forceDamage;
    void Start()
    {
        playerStatus = PlayerStatus.Instance;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            playerStatus.Health(damage, forceDamage);
        }    
    }
}
