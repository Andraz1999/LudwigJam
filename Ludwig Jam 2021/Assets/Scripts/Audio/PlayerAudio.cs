using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] AudioManager audioManager;


    #region Singleton
    public static PlayerAudio Instance {get; private set;}
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }   
    #endregion
    private void Start() {
        player = PlayerMovement.Instance;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void walkAudio()
    {   
        //Debug.Log("LALALA");
        if(player.CheckIfSwitched())
            {
                //audioManager.RandomPlay("walkTrava", 1f, 1f, 0.8f, 1.1f);
                audioManager.Play("walkTrava");
            }
        else
        {
            //audioManager.RandomPlay("walkBeton", 1f, 1f, 0.8f, 1.1f);
            audioManager.Play("walkBeton");
        }
    }

    public void jumpAudio()
    {   
        if(player.CheckIfSwitched())
            {
                //audioManager.RandomPlay("jumpTrava", 1f, 1f, 0.8f, 1.1f);
                audioManager.Play("jumpTrava");
            }
        else
        {
            //audioManager.RandomPlay("jumpBeton", 1f, 1f, 0.8f, 1.1f);
            audioManager.Play("jumpBeton");
        }
    }

}
