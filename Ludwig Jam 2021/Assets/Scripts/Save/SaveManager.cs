using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
	// Start is called before the first frame update
	[HideInInspector]public int checkpoint;
	[HideInInspector]public float time;
	public float bestTime;
    public string bestTimeString;

	PlayerStatus playerStatus;

	#region Singleton
    public static SaveManager Instance {get; private set;}
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

    }   
    #endregion

	private void Start() {
		
	}

	public void Save()
	{
		playerStatus = PlayerStatus.Instance;
		checkpoint = playerStatus.checkpoint;
		time = playerStatus.time;
		bestTime = playerStatus.bestTime;
		bestTimeString = playerStatus.bestTimeString;

		SaveSystem.Save(this);

		Debug.Log("Successfully saved");
	}

	public void Load()
	{
		playerStatus = PlayerStatus.Instance;
		PlayerData data = SaveSystem.Load();

		playerStatus.time = data.time;
		playerStatus.checkpoint = data.checkpoint;
		playerStatus.bestTime = data.bestTime;
		playerStatus.bestTimeString = data.bestTimeString;

		Debug.Log("Successfully loaded");
	}

	
}
