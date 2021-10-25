using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int conditions;
    //[SerializeField] float responeTime = 15f;
    public float pauseFor = 2f;
    public  int currentConditions;
    private Animator animator;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        CloseDoor();
        audioManager = AudioManager.Instance;
    }

    public void IncreaseCondition(int n)
    {
        currentConditions += n;
        if(currentConditions >= conditions)
        {
            audioManager.Play("youDidIt");
            Invoke("OpenDoor", pauseFor); 
        }
           
    }

    void OpenDoor()
    {
        animator.SetBool("isOpen", true);
        //Invoke("CloseDoor", responeTime);
    }
    void CloseDoor()
    {
        animator.SetBool("isOpen", false);
        currentConditions = 0;
    }

    public void goingUpAudio()
    {
        audioManager.Play("doorUp");
    }
    public void goingUpAudioStop()
    {
        audioManager.StopPlaying("doorUp");
    }
    public void OpenAudio()
    {
        audioManager.Play("open");
    }

}
