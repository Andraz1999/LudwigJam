using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiky : MonoBehaviour
{
    public bool isFlip = true;
    public float interval;
    void Start()
    { 
        if(isFlip) InvokeRepeating("Flip", 5.0f, interval);
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
    }
}
