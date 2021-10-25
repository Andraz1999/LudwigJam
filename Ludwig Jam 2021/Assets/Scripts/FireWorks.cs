using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorks : MonoBehaviour
{
    [SerializeField] GameObject fireWorks;
    [SerializeField] Transform fwPosition;
    bool active = true;

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && active)
        {
            Destroy(Instantiate(fireWorks, fwPosition.position, transform.rotation), 5f);
            active = false;
        }
    }
}
