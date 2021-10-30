using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    PlayerStatus playerStatus;
    public int checkpoint;
    public bool startSwitch;
    public bool isActive;
    void Awake()
    {
        isActive = true;
    }
    void Start()
    {
        playerStatus = PlayerStatus.Instance;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && isActive)
        {
            playerStatus.checkpoint = this.checkpoint;
        }        
    }
}
