using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] GameObject go;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        go.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        go.SetActive(false);
    }
}
