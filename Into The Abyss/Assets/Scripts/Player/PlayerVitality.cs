using System;
using System.Collections;
using UnityEngine;

public class PlayerVitality : MonoBehaviour, IDamageable, IHealable
{
	[Header("Player Components"), Space]
	[SerializeField] private Stats stats;
	[SerializeField] private MonoBehaviour movementScript;

	[Header("References"), Space]
	[SerializeField] private Rigidbody2D rb2D;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private float damageFlashTime;

	public Vector2 Position => throw new NotImplementedException();
	public bool CanBeHealed => throw new NotImplementedException();
	
	public static event Action<int> OnHealthChanged;

	private Material _mat;
	private int _maxHealth;
	private int _currentHealth;
	private float _invincibilityTime;

	private void Start()
	{
		_maxHealth = (int)stats.GetDynamicStat(Stat.MaxHealth);
		_currentHealth = _maxHealth;
		_mat = spriteRenderer.material;
		
		OnHealthChanged?.Invoke(_currentHealth);
	}

	private void Update()
	{
		if (_invincibilityTime > 0f)
		{
			_invincibilityTime -= Time.deltaTime;
		}
	}

	public void TakeDamage(Stats attackerStats, Vector3 attackerPos, float scaleFactor = 1)
	{
		if (_invincibilityTime <= 0f)
		{
			_invincibilityTime = stats.GetStaticStat(Stat.InvincibilityTime);

			_currentHealth -= (int)(attackerStats.GetDynamicStat(Stat.AttackDamage) * scaleFactor);
			_currentHealth = Mathf.Max(0, _currentHealth);

			OnHealthChanged?.Invoke(_currentHealth);

			StartCoroutine(TriggerDamageFlash());
			StartCoroutine(BeingKnockedBack(attackerPos, attackerStats.GetStaticStat(Stat.KnockBackStrength)));

			if (_currentHealth <= 0)
			{
				Die();
			}
			else
			{
				AudioManager.Instance.Play("Injured");
			}
		}
	}

	public void Heal(int amount)
	{
		if (_currentHealth > 0)
		{
			_currentHealth += amount;
			_currentHealth = Mathf.Min(_currentHealth, _maxHealth);

			OnHealthChanged?.Invoke(_currentHealth);
		}
	}

	private void Die()
	{
		GameManager.Instance.GameOver();
		gameObject.SetActive(false);
	}

	private IEnumerator BeingKnockedBack(Vector3 attackerPos, float strength)
	{
		if (attackerPos == default)
			yield break;

		rb2D.linearVelocity = Vector3.zero;
		movementScript.enabled = false;

		Vector2 direction = transform.position - attackerPos;
		float knockBackStrength = strength * (1f - stats.GetStaticStat(Stat.KnockBackRes));

		Vector2 force = direction.normalized * knockBackStrength;

		rb2D.AddForce(force, ForceMode2D.Impulse);

		yield return new WaitForSeconds(.25f);

		movementScript.enabled = true;
	}

	private IEnumerator TriggerDamageFlash()
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
