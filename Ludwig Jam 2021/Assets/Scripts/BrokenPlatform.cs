using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    [SerializeField] float respawnTime = 10f;
    [SerializeField] float pauseFor = 5f;
    SpriteRenderer sprite;
    [SerializeField] GameObject gfx2;
    [SerializeField] GameObject destroyEffect;
    [SerializeField] Collider2D colTriger;
    [SerializeField] Collider2D col1;


    private void Start() 
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            gfx2.SetActive(true);
            sprite.enabled = false;
            colTriger.enabled = false;
            Invoke("Fall", pauseFor);
        }
    }

    void Fall()
    {
        gfx2.SetActive(false);
        col1.enabled = false;
        Destroy(Instantiate(destroyEffect, transform.position, transform.rotation), 5f);
        Invoke("Respawn", respawnTime);
    }
    void Respawn()
    {
        col1.enabled = true;
        sprite.enabled = true;
        colTriger.enabled = true;
    }
}
