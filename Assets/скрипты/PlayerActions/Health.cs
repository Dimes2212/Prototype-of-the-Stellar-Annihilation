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
    public TextMeshProUGUI hologramText;

    [Header("Награда за убийство")]
    public int rewardOnDeath = 0;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHologram();
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        onDamage?.Invoke();
        UpdateHologram();

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHologram();
    }

    void Die()
    {
        onDeath?.Invoke();
        Debug.Log($"{name} погиб. RewardOnDeath = {rewardOnDeath}");

        if (rewardOnDeath > 0)
        {
            // Находим игрока по тегу и достаём компонент
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                var currency = playerObj.GetComponent<PlayerCurrency>();
                if (currency != null)
                {
                    currency.AddCurrency(rewardOnDeath);
                    Debug.Log($"Добавлено {rewardOnDeath} валюты игроку.");
                }
                else
                {
                    Debug.LogWarning("Health.Die: на объекте Player нет PlayerCurrency!");
                }
            }
            else
            {
                Debug.LogWarning("Health.Die: объект с тегом Player не найден!");
            }
        }

        Destroy(gameObject);
    }

    void UpdateHologram()
    {
        if (hologramText == null) return;
        hologramText.text = $"HP: {Mathf.Ceil(currentHealth)}";
        hologramText.color = currentHealth <= maxHealth * 0.3f ? Color.red : Color.white;
    }

    public float GetHealth() => currentHealth;
    public float GetHealthNormalized() => currentHealth / maxHealth;
}
