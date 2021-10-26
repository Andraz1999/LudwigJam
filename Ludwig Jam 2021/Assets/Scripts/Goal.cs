using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    PlayerStatus playerStatus;
    void Start()
    {
        playerStatus = PlayerStatus.Instance;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            playerStatus.Goal();
        }
    }
}
