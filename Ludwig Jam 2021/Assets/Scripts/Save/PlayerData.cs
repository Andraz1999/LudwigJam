using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public int checkpoint;
    public float time;
    public float bestTime;
    public string bestTimeString;

    public PlayerData(SaveManager saveManager)
    {
        checkpoint = saveManager.checkpoint;
        time = saveManager.time;
        bestTime = saveManager.bestTime;
        bestTimeString = saveManager.bestTimeString;
    }

    public PlayerData()
    {
        checkpoint = -1;
        time = 0;
        bestTime = 0f;
        bestTimeString = "";
    }
}
