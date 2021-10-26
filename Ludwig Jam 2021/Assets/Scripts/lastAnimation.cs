using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lastAnimation : MonoBehaviour
{
    public float timeScalekkoeficijent;

    private void Start() {
        timeScalekkoeficijent = 1;
    }
    void Update()
    {
        Time.timeScale = timeScalekkoeficijent;
    }
}
