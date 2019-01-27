using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
	private static GameState instance;

	public static GameState Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameState>();
				if (instance == null)
				{
					GameObject go = new GameObject("GameStateSingleton");
					instance = go.AddComponent<GameState>();
				}
			}

			return instance;
		}
	}

	public PlayerState player = new PlayerState();
}
