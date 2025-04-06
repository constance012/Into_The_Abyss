using System;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    public static event Action<int> OnHealthChange;
    public static event Action OnPlayerDie;

    [Header("Health")]
    public int maxHealth = 3;
    private int _health;
    public int currentHealth { 
        get{
            return _health;
        }
        private set{
            _health = value;

            if(_health > maxHealth)
            {
                _health = maxHealth;
            }

            if(_health <= 0)
            {
                _health = 0;
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        OnPlayerDie?.Invoke();
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChange?.Invoke(currentHealth);
        Debug.Log("currentHealth: " + currentHealth);
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        OnHealthChange?.Invoke(currentHealth);
        Debug.Log("currentHealth: " + currentHealth);
    }
}
