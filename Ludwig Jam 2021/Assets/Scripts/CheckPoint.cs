using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    PlayerStatus playerStatus;
    public int checkpoint;
    [HideInInspector]public bool startSwitch;
    public bool isActive;
    public int progress;
    public bool isCameraWide;
    public Animator animator;
    AudioManager audioManager;
    void Awake()
    {
        isActive = true;
    }
    void Start()
    {
        playerStatus = PlayerStatus.Instance;
        audioManager = AudioManager.Instance;
        animator.SetBool("isOpen", false);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && isActive)
        {
            playerStatus.checkpoint = this.checkpoint;
            animator.SetBool("isOpen", true);
            audioManager.Play("playButton");
            isActive = false;
        }        
    }
}
