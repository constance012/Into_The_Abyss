using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TextMeshProUGUI HPText;

    private void UpdateHealthPoint(int currentHealth, Vector2 knockbackForce)
    {
        HPText.text = "HP: " + currentHealth;
    }

    void OnEnable()
    {
        HealthPoint.OnHealthChange += UpdateHealthPoint;
    }
    void OnDisable()
    {
        HealthPoint.OnHealthChange -= UpdateHealthPoint;
    }
}