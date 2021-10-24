using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectBalls : MonoBehaviour
{
    [SerializeField] float respawnTime = 15f;
    [SerializeField] GameObject collectEffect;
    [SerializeField] UnityEvent onComplete;
    [SerializeField] GameObject gfx;
    
    //SpriteRenderer sprite;
    Collider2D col;
    
    private void Start() 
    {
        //sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();    
    }

    void Respawn()
    {
        //sprite.enabled = true;
        gfx.SetActive(true);
        col.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            //sprite.enabled = false;
            gfx.SetActive(false);
            col.enabled = false;
            onComplete.Invoke();
            Invoke("Respawn", respawnTime);
            Destroy(Instantiate(collectEffect, transform.position, transform.rotation), 5f);
        }    
    }
}
