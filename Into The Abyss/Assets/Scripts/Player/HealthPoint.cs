using System;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
	[Header("Stats"), Space]
	[SerializeField] private Stats stats;

	public static event Action<int, Vector2> OnHealthChange;

	public int CurrentHealth { 
		get{
			return _health;
		}
		private set{
			_health = value;

			if(_health > _maxHealth)
			{
				_health = _maxHealth;
			}

			if(_health <= 0)
			{
				_health = 0;
				Debug.Log("Player is dead!");
				GameManager.Instance.GameOver();
			}
		}
	}

	private int _maxHealth;
	private int _health;

	private void Start()
	{
		_maxHealth = (int)stats.GetDynamicStat(Stat.MaxHealth);
		ChangeHealth(_maxHealth, Vector2.zero);
	}

	public void ChangeHealth(int change, Vector2 knockback)
	{
		CurrentHealth += change;

		OnHealthChange?.Invoke(CurrentHealth, knockback);
	}
}
