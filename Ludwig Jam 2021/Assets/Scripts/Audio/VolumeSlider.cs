using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
   // public Slider backVolumeSlider;
   // public Slider effectVolumeSlider;

   public Slider volumeSlider;
   AudioManager audioManager;

//    private VolumeSlider instance;

   private void Awake() 
   {
    //    if(instance == null)
    //         instance = this;
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    //     DontDestroyOnLoad(gameObject);

    audioManager = AudioManager.Instance;
   }

   private void Start()
   {
      volumeSlider.value = audioManager.koeficijent;
   }

   private void Update() 
   {
   //     FindObjectOfType<AudioManager>().BackVolumeAdjust(backVolumeSlider.value);
   //     FindObjectOfType<AudioManager>().EffectVolumeAdjust(effectVolumeSlider.value);

      FindObjectOfType<AudioManager>().VolumeAdjust(volumeSlider.value);
   }
}
