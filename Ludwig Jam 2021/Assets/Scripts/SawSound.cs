using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawSound : MonoBehaviour
{
    public AudioSource audioSource;
    private  float asVolume;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.Instance;
        asVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = asVolume * audioManager.koeficijent;
    }
}
