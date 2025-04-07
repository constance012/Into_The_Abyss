using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public static event Action OnGameOver;
	public static event Action OnGameRetry;
	public static event Action OnGameVictory;

	public bool GameDone { get; private set; }

	public void GameOver()
	{
		Debug.Log("Game Over!");

		GameDone = true;
		OnGameOver?.Invoke();
	}

	public void GameRetry()
	{
		Debug.Log("Game Retry!");
		SceneLoader.Instance.LoadSceneAsync("Scenes/Main Gameplay");
		
		GameDone = false;
		OnGameRetry?.Invoke();
	}

	public void GameVictory()
	{
		Debug.Log("Game Victory!");
		
		GameDone = true;
		OnGameVictory?.Invoke();
	}

	public void BackToMenu()
	{
		GameDone = false;
		SceneLoader.Instance.LoadSceneAsync("Scenes/Main Menu");
	}
}