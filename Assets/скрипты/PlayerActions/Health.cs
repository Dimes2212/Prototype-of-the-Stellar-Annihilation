using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    
    public float maxHealth = 100f;
    private float currentHealth;

    
    public UnityEvent onDamage;
    public UnityEvent onDeath;

    
    public TextMeshProUGUI hologramText;

    
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
