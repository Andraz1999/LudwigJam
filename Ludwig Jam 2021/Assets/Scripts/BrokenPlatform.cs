using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    [SerializeField] float respawnTime = 10f;
    [SerializeField] float pauseFor = 5f;
    private Animator animator;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            animator.SetTrigger("stepOn");
            Invoke("Fall", pauseFor);
        }
    }

    void Fall()
    {
        animator.SetTrigger("falling");
        Invoke("Respawn", respawnTime);
    }
    void Resapwn()
    {
        animator.SetTrigger("respawn");
    }
}
