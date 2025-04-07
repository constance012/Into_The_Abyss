using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public static event Action OnGameOver;
	public static event Action OnGameRetry;
	public static event Action OnGameVictory;

	public bool GameDone { get; private set; }
	public bool GameStarted { get; private set; }
	public float CurrentDepthInMeter => _currentDepth;

	private float _currentDepth;

	private void Start()
	{
		CameraSwitcher.Instance.Switch(CameraType.SurfaceCamera);	
	}

	private void Update()
	{
		MonitorDepth();
	}

	private void MonitorDepth()
	{
		_currentDepth = PlayerController.Position.y;

		if (_currentDepth < -10f && !GameStarted)
		{
			CameraSwitcher.Instance.Switch(CameraType.DigCamera);
			GameStarted = true;
		}
	}

	public void GameOver()
	{
		Debug.Log("Game Over!");

		GameDone = true;
		GameStarted = false;
		OnGameOver?.Invoke();
	}

	public void GameRetry()
	{
		Debug.Log("Game Retry!");
		SceneLoader.Instance.LoadSceneAsync("Scenes/Main Gameplay");
		
		GameDone = false;
		GameStarted = false;
		OnGameRetry?.Invoke();
	}

	public void GameVictory()
	{
		Debug.Log("Game Victory!");
		
		GameDone = true;
		GameStarted = false;
		OnGameVictory?.Invoke();
	}

	public void BackToMenu()
	{
		GameDone = false;
		GameStarted = false;
		SceneLoader.Instance.LoadSceneAsync("Scenes/Main Menu");
	}
}