using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressUpdate : MonoBehaviour
{
    private Progressbar progressbar;
    [SerializeField] int progress;

    void Start()
    {
        progressbar = Progressbar.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            progressbar.SetProgress(progress);
        }    
    }
}
