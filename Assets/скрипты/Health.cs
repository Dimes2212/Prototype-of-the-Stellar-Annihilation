using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    [Header("Настройки здоровья")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("События")]
    public UnityEvent onDamage;
    public UnityEvent onDeath;

    [Header("UI (голограмма)")]
    public TextMeshProUGUI hologramText; // Ссылка на текст в Canvas

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHologram();
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        onDamage?.Invoke();
        UpdateHologram();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        UpdateHologram();
    }

    void Die()
    {
        onDeath?.Invoke();
        Debug.Log($"{gameObject.name} погиб.");
        // Здесь можно отключить объект, анимации или смерть
    }

    void UpdateHologram()
    {
        if (hologramText != null)
        {
            hologramText.text = $"HP: {Mathf.Ceil(currentHealth)}";

            // Пример: сделать текст красным при низком HP
            if (currentHealth <= maxHealth * 0.3f)
                hologramText.color = Color.red;
            else
                hologramText.color = Color.white;
        }
    }

    // Полезные геттеры
    public float GetHealth() => currentHealth;
    public float GetHealthNormalized() => currentHealth / maxHealth;
}
