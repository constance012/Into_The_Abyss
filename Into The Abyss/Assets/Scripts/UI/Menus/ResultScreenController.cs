using TMPro;
using UnityEngine;

public class ResultScreenController : MonoBehaviour
{
	[Header("Screen References"), Space]
	[SerializeField] private TweenableUIMaster gameOverScreen;
	[SerializeField] private TweenableUIMaster victoryScreen;

	[Header("UI References"), Space]
	[SerializeField] private TextMeshProUGUI chestCountText;

	[Header("Settings"), Space]
	[SerializeField] private int totalChest;

	private int _chestOpened;

	private void Start()
	{
		_chestOpened = 0;	
	}

	private void OnEnable()
	{
		UnsubscribeEvents();
		SubscribeEvents();
	}

	private void OnDisable()
	{
		UnsubscribeEvents();
	}

	public void SubscribeEvents()
	{
		GameManager.OnGameOver += GameManager_OnGameOver;
		GameManager.OnGameVictory += GameManager_OnGameVictory;
		GameManager.OnGameRetry += GameManager_OnGameRetry;

		Chest.OnChestOpened += Chest_OnChestOpened;
	}
	
	public void UnsubscribeEvents()
	{
		GameManager.OnGameOver -= GameManager_OnGameOver;
		GameManager.OnGameVictory -= GameManager_OnGameVictory;
		GameManager.OnGameRetry -= GameManager_OnGameRetry;
		
		Chest.OnChestOpened -= Chest_OnChestOpened;
	}

	public void Chest_OnChestOpened()
	{
		_chestOpened++;
	}

	private async void GameManager_OnGameOver()
	{
		await gameOverScreen.SetActive(true);
	}
	
	private async void GameManager_OnGameVictory()
	{
		chestCountText.text = $"{_chestOpened} / {totalChest}";

		await victoryScreen.SetActive(true);
	}
	
	private async void GameManager_OnGameRetry()
	{
		await gameOverScreen.SetActive(false);
		await victoryScreen.SetActive(false);
	}
}