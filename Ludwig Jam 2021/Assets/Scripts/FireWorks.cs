using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorks : MonoBehaviour
{
    [SerializeField] GameObject fireWorks;
    //[SerializeField] Transform fwPosition;
    bool active = true;

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && active)
        {
            fireWorks.SetActive(true);
            StartCoroutine("delay");
            
            active = false;
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(5f);
        fireWorks.SetActive(false);
    }
}
