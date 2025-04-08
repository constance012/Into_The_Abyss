using System;
using System.Collections;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
	[Header("Stats"), Space]
	[SerializeField] private Stats stats;

	[Header("References"), Space]
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private float damageFlashTime;

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

	private Material _mat;
	private int _maxHealth;
	private int _health;

	private void Start()
	{
		_maxHealth = (int)stats.GetDynamicStat(Stat.MaxHealth);
		_mat = spriteRenderer.material;
		ChangeHealth(_maxHealth, Vector2.zero);
	}

	public void ChangeHealth(int change, Vector2 knockback)
	{
		CurrentHealth += change;

		if (change < 0)
		{
			AudioManager.Instance.Play("Injured");
			StartCoroutine(TriggerDamageFlash());
		}
		else
		{
			AudioManager.Instance.Play("Healing");
		}

		OnHealthChange?.Invoke(CurrentHealth, knockback);
	}

	protected IEnumerator TriggerDamageFlash()
	{
		float flashIntensity;
		float elapsedTime = 0f;

		while (elapsedTime < damageFlashTime)
		{
			elapsedTime += Time.deltaTime;

			flashIntensity = Mathf.Lerp(1f, 0f, elapsedTime / damageFlashTime);
			_mat.SetFloat("_FlashIntensity", flashIntensity);

			yield return null;
		}
	}
}
