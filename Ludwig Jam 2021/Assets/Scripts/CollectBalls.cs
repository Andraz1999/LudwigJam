using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectBalls : MonoBehaviour
{
    [SerializeField] float respawnTime = 15f;
    [SerializeField] bool shouldRespawnAfterTime;
    [SerializeField] GameObject collectEffect;
    [SerializeField] UnityEvent onComplete;
    [SerializeField] GameObject gfx;

    private AudioManager audioManager;
    
    //SpriteRenderer sprite;
    Collider2D col;
    
    private void Start() 
    {
        //sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();    
        audioManager = AudioManager.Instance;
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
            audioManager.Play("pickup");
            gfx.SetActive(false);
            col.enabled = false;
            onComplete.Invoke();
            if(shouldRespawnAfterTime)
                Invoke("Respawn", respawnTime);
            Destroy(Instantiate(collectEffect, transform.position, transform.rotation), 5f);
        }    
    }
}
