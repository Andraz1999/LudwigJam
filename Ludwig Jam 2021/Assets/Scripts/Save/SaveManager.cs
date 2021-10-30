﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
	// Start is called before the first frame update
	public int checkpoint;
	public float time;

	PlayerStatus playerStatus;

	private void Start() {
		playerStatus = PlayerStatus.Instance;
	}

	public void Save()
	{
		// checkpoint = playerStatus.checkpoint;
		// time = playerStatus.time;

		SaveSystem.Save(this);
	}

	public void Load()
	{
		PlayerData data = SaveSystem.Load();

		// playerStatus.time = data.time;
		// playerStatus.checkpoint = data.checkpoint;
	}

	
}