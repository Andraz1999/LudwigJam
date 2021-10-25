using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float volume;
    public Sounds[] sounds;
    public List<AudioSource> audioSources;
    [SerializeField] private bool doublePart;

    public int backgroundSoundNumber;
    public float backgroundVolume;
    public float effectVolume;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        // if(instance == null)
        //     instance = this;
        // else
        // {
        //     Destroy(gameObject);
        // }
        // DontDestroyOnLoad(gameObject);


        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            audioSources.Add(s.source);
        }
    }

     private void Start() 
   {
        if(!doublePart)
            Play("MenuTheme");
        else
            StartCoroutine("DoubleSound");
   }

   public void Play(string name)
   {
       Sounds s = Array.Find(sounds, sound => sound.name == name);
       if(s == null)
       {
           Debug.Log("Sound: " + name + " not found, napiši ime prav!");
           return;
       }
        s.source.Play();
   }

   public void PlayNotForced(string name)
   {
       Sounds s = Array.Find(sounds, sound => sound.name == name);
       if(s == null || s.source.isPlaying)
       {
           Debug.Log("Sound: " + name + " not found, napiši ime prav!");
           return;
       }
        s.source.Play();
   }

   public void RandomPlay(string name, float minVol, float maxVol, float minPitch, float maxPitch)
   {       
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
       {
           Debug.Log("Sound: " + name + " not found, napiši ime prav!");
           return;
       }
        s.source.volume = UnityEngine.Random.Range(minVol, maxVol);
        s.source.volume *= volume;
        s.source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        s.source.Play();

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        s.source.volume *= volume;
    }

    public void StopPlaying(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
       {
           Debug.Log("Sound: " + name + " not found, napiši ime prav!");
           return;
       }
        s.source.Stop();
    }

    private IEnumerator DoubleSound()
        {
            Play("FirstPart");
            Sounds s = Array.Find(sounds, sound => sound.name == "FirstPart");
            Debug.Log(s.clip.length);
            yield return new WaitForSeconds(s.clip.length - 0.10775f);
            StopPlaying("FirstPart");
            Play("SecondPart");
        }

    // private void Update()
    // {
    //     Slider backgroundVolumeSlider = GameObject.Find("backgroundVolumeSlider").GetComponent<Slider>();
    //     if(backgroundVolumeSlider != null)
    //         {
    //             backgroundVolume = backgroundVolumeSlider.value;
    //             for (int i = 0; i <= backgroundSoundNumber; i++)
    //             {
    //                 audioSources[i].volume = backgroundVolume;
    //             }

    //         }

    //         Slider effectVolumeSlider = GameObject.Find("effectVolumeSlider").GetComponent<Slider>();
    //         if(effectVolumeSlider != null)
    //         {
    //             effectVolume = effectVolumeSlider.value;
    //             for (int i = audioSources.Count; i > backgroundSoundNumber; i--)
    //             {
    //                 audioSources[i].volume = effectVolume;
    //             }

    //         }
    // }

    public void PitchChange(float koeficijent)
    {
        for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].pitch *= koeficijent;
            }
        
        
    }

    // public void EffectVolumeAdjust(float koeficijent)
    // {
    //     for (int i = audioSources.Count - 1; i > backgroundSoundNumber; i--)
    //     {
    //         audioSources[i].volume = sounds[i].volume * koeficijent;
    //     }
        
    // }

    // public void BackVolumeAdjust(float koeficijent)
    // {
    //     for (int i = 0; i <= backgroundSoundNumber; i++)
    //     {
    //         audioSources[i].volume = sounds[i].volume * koeficijent;
    //     }
        
    // }

     public void VolumeAdjust(float koeficijent)
    {
        for (int i = 0; i <= audioSources.Count - 1; i++)
        {
            audioSources[i].volume = sounds[i].volume * koeficijent;
        }
        
    }
}
