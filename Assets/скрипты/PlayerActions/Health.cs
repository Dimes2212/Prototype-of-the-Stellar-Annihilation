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

    // Ссылка на Animator для врага
    private Animator animator;

    // Параметр для анимации смерти
    private static readonly int IsDead = Animator.StringToHash("isDead");

    // Ссылка на EnemyStateManager
    private EnemyStateManager enemyStateManager;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Получаем Animator компонента
        enemyStateManager = GetComponent<EnemyStateManager>(); // Получаем ссылку на EnemyStateManager
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
        
        if (animator != null)
        {
            animator.SetBool(IsDead, true);  
        }

        
        onDeath?.Invoke();

        
        if (enemyStateManager != null)
        {
            enemyStateManager.Die();  
        }

        
        Destroy(gameObject, 5f);  
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
