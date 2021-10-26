using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorks : MonoBehaviour
{
    [SerializeField] GameObject fireWorks;
    //[SerializeField] Transform fwPosition;
    bool active = true;

    private AudioManager audioManager;

    private void Start() {
        audioManager = AudioManager.Instance;
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && active)
        {
            fireWorks.SetActive(true);
            active = false;
            audioManager.Play("fireworksTrail");
            Invoke("PlayExplosion", 2f);
            StartCoroutine("delay");
            
        }
    }
    private void PlayExplosion()
    {
        audioManager.Play("fireworksExplosion");
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(5f);
        fireWorks.SetActive(false);
    }
}
