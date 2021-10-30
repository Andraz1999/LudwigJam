using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{ 
    [SerializeField] TextMeshProUGUI timeCounter;

    private TimeSpan timePlaying;
    private bool timerGoing;

    private float elapsedTime;
    private string timePlayingStr;
    
   #region Singleton
    public static TimerController Instance {get; private set;}
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }   
    #endregion

    void Start()
    {
        timeCounter.SetText("Time: 00:00.00");
        timerGoing = false;
        // Here add for connecting between Scenes.
        BeginTimer();
    }

    public void BeginTimer(float startTime = 0f)
    {
        timerGoing = true;
        //var startTime = Time.time;
        elapsedTime = startTime;

        StartCoroutine(UpdateTimer());
    }


    public void EndTimer()
    {
        timerGoing = false;
    }

    public void RestartTimer()
    {
        timeCounter.SetText("Time: 00:00.00");
        timerGoing = false;
        BeginTimer(); 
    }

    public float GetTime()
    {
        return (float) timePlaying.TotalSeconds;
    }
    public string GetTimeString()
    {
        return  timePlayingStr;
    }

    
    IEnumerator UpdateTimer()
    {
        while(timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.SetText("Time: " + timePlayingStr);
            
            yield return null;
        }
    }
   


}
