using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int conditions;
    [SerializeField] float responeTime = 15f;
    public float pauseFor = 2f;
    private int currentConditions;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        CloseDoor();
    }

    public void IncreaseCondition(int n)
    {
        currentConditions += n;
        if(currentConditions >= conditions)
        Invoke("OpenDoor", pauseFor);    
    }

    void OpenDoor()
    {
        animator.SetBool("isOpen", true);
        Invoke("CloseDoor", responeTime);
    }
    void CloseDoor()
    {
        animator.SetBool("isOpen", false);
        currentConditions = 0;
    }

}
