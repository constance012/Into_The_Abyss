using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public static event Action OnGameOver;
	public static event Action OnGameRetry;
	public static event Action OnGameVictory;

	public void GameOver()
	{
		Debug.Log("Game Over!");
		OnGameOver?.Invoke();
	}

	public void GameRetry()
	{
		Debug.Log("Game Retry!");
		OnGameRetry?.Invoke();
	}

	public void GameVictory()
	{
		Debug.Log("Game Victory!");
		OnGameVictory?.Invoke();
	}
}