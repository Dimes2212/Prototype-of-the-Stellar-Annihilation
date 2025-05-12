//using UnityEngine;
//using UnityEngine.Events;
//using TMPro;

//public class Health : MonoBehaviour
//{
//    public float maxHealth = 100f;
//    private float currentHealth;

//    public UnityEvent onDamage;
//    public UnityEvent onDeath;

//    public TextMeshProUGUI hologramText;

//    public int rewardOnDeath = 0;


//    private Animator animator;


//    private static readonly int IsDead = Animator.StringToHash("isDead");


//    private EnemyStateManager enemyStateManager;
//    private SimpleEnemyStateManager simpleenemyStateManager;

//    void Awake()
//    {
//        currentHealth = maxHealth;
//        animator = GetComponent<Animator>(); 
//        enemyStateManager = GetComponent<EnemyStateManager>();
//        simpleenemyStateManager = GetComponent<SimpleEnemyStateManager>();
//        UpdateHologram();
//    }

//    public void TakeDamage(float amount)
//    {
//        if (currentHealth <= 0) return;

//        currentHealth = Mathf.Max(currentHealth - amount, 0f);
//        onDamage?.Invoke();
//        UpdateHologram();

//        if (currentHealth <= 0)
//            Die();
//    }

//    public void Heal(float amount)
//    {
//        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
//        UpdateHologram();
//    }

//    void Die()
//    {

//        if (animator != null)
//        {
//            animator.SetBool(IsDead, true);  
//        }


//        onDeath?.Invoke();
//        var enemyManager = GetComponent<EnemyStateManager>();
//        var simpleEnemyManager = GetComponent<SimpleEnemyStateManager>();

//        enemyManager?.Die();
//        simpleEnemyManager?.Die();


//        if (enemyStateManager != null)
//        {
//            enemyStateManager.Die();  
//        }

//        if (simpleenemyStateManager != null)
//        {
//            simpleenemyStateManager.Die();
//        }


//        Destroy(gameObject, 5f);  
//    }

//    void UpdateHologram()
//    {
//        if (hologramText == null) return;
//        hologramText.text = $"HP: {Mathf.Ceil(currentHealth)}";
//        hologramText.color = currentHealth <= maxHealth * 0.3f ? Color.red : Color.white;
//    }

//    public float GetHealth() => currentHealth;
//    public float GetHealthNormalized() => currentHealth / maxHealth;
//}


using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public UnityEvent onDamage;
    public UnityEvent onDeath;

    [Header("UI")]
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

        // Запускаем анимацию смерти
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Вызываем соответствующую логику смерти
        if (simpleEnemyStateManager != null)
        {
            simpleEnemyStateManager.Die();
        }
        else if (enemyStateManager != null)
        {
            enemyStateManager.Die();
        }
        else
        {
            // Если нет стейт-менеджера, просто уничтожаем объект
            Destroy(gameObject, 3f);
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