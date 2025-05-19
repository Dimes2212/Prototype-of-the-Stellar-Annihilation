using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement; // Добавляем пространство имён для работы со сценами

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public UnityEvent onDamage;
    public UnityEvent onDeath;
    public TextMeshProUGUI hologramText;
    public int rewardOnDeath = 0;
    [SerializeField] private string deathSceneName = "GameOver"; // Имя сцены для перехода при смерти
    
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
            animator.Play("Death", -1, 0f);
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

        // Переход на сцену смерти, если здоровье равно 0
        if (currentHealth <= 0)
        {
            LoadDeathScene();
        }
    }

    void LoadDeathScene()
    {
        SceneManager.LoadScene("deathSceneName");
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