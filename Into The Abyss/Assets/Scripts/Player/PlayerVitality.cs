using System;
using System.Collections;
using UnityEngine;

public class PlayerVitality : MonoBehaviour, IDamageable, IHealable
{
	[Header("Player Components"), Space]
	[SerializeField] private Stats stats;
	[SerializeField] private PlatformerController movementScript;

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

			AudioManager.Instance.Play("Injured");
			StartCoroutine(TriggerDamageFlash());
			StartCoroutine(BeingKnockedBack(attackerPos, attackerStats.GetStaticStat(Stat.KnockBackStrength)));

			if (_currentHealth <= 0)
			{
				Die();
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

	private Vector2 OffsetStraightUpKnockBackDirection(Vector2 direction, float minAngle, float maxAngle)
	{
		float angle = Vector2.Angle(direction, Vector2.right);

		if (angle >= minAngle && angle <= maxAngle)
		{
			float sign = angle <= 90f ? -1f : 1f;
			direction = Quaternion.Euler(0f, 0f, 40f * sign) * direction;
		}

		return direction;
	}

	private IEnumerator BeingKnockedBack(Vector3 attackerPos, float strength)
	{
		if (attackerPos == default)
			yield break;


		rb2D.linearVelocity = Vector2.zero;
		movementScript.SetEnable(false);

		Vector2 direction = (transform.position - attackerPos).normalized;
		direction = OffsetStraightUpKnockBackDirection(direction, 80f, 120f);

		float knockBackStrength = strength * (1f - stats.GetStaticStat(Stat.KnockBackRes));

		Vector2 force = direction * knockBackStrength;

		rb2D.AddForce(force, ForceMode2D.Impulse);

		yield return new WaitForSeconds(.25f);

		movementScript.SetEnable(true);
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
