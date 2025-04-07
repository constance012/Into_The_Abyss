using System;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    public static event Action<int, Vector2> OnHealthChange;

    [Header("Health"), Space]
    [SerializeField] public int maxHealth = 3;
    private int _health;
    public int CurrentHealth { 
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
                Debug.Log("Player is dead!");
            }
        }
    }

    private void Start()
    {
        ChangeHealth(maxHealth, Vector2.zero);
    }

    public void ChangeHealth(int change, Vector2 knockback)
    {
        CurrentHealth += change;

        OnHealthChange?.Invoke(CurrentHealth, knockback);
    }
}
