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

    private Animator animator;
    private SimpleEnemyStateManager simpleEnemyStateManager;
    private EnemyStateManager enemyStateManager;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        simpleEnemyStateManager = GetComponent<SimpleEnemyStateManager>();
        enemyStateManager = GetComponent<EnemyStateManager>();
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

    void Die()
    {
        onDeath?.Invoke();

        if (animator != null)
        {
            animator.ResetTrigger("Die");
            animator.SetTrigger("Die");
        }

        if (simpleEnemyStateManager != null)
        {
            simpleEnemyStateManager.Die();
        }
        else if (enemyStateManager != null)
        {
            enemyStateManager.Die();
        }
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