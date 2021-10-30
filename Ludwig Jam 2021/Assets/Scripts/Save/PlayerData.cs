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

    public PlayerData(SaveManager saveManager)
    {
        checkpoint = saveManager.checkpoint;
        time = saveManager.time;
    }
}
