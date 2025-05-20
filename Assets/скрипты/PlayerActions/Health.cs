//using UnityEngine;
//using UnityEngine.Events;
//using TMPro;
//using UnityEngine.SceneManagement; 

//public class Health : MonoBehaviour
//{
//    public float maxHealth = 100f;
//    private float currentHealth;
//    public UnityEvent onDamage;
//    public UnityEvent onDeath;
//    public TextMeshProUGUI hologramText;
//    public int rewardOnDeath = 0;
//    [SerializeField] private string deathSceneName = "GameOver"; 
//    private Animator animator;
//    private SimpleEnemyStateManager simpleEnemyStateManager;
//    private EnemyStateManager enemyStateManager;

//    void Awake()
//    {
//        currentHealth = maxHealth;
//        animator = GetComponent<Animator>();
//        simpleEnemyStateManager = GetComponent<SimpleEnemyStateManager>();
//        enemyStateManager = GetComponent<EnemyStateManager>();
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

//    void Die()
//    {
//        onDeath?.Invoke();

//        if (animator != null)
//        {
//            animator.Play("Death", -1, 0f);
//            animator.ResetTrigger("Die");
//            animator.SetTrigger("Die");
//        }

//        if (simpleEnemyStateManager != null)
//        {
//            simpleEnemyStateManager.Die();
//        }
//        else if (enemyStateManager != null)
//        {
//            enemyStateManager.Die();
//        }


//        if ((currentHealth <= 0) && (simpleEnemyStateManager == null) && (enemyStateManager == null))
//        {

//        }
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
    public TextMeshProUGUI hologramText;
    public int rewardOnDeath = 0;

    [SerializeField] private EnemyZoneCleaner enemyZoneCleaner;

    [SerializeField] private Transform deathZonePoint;     // Точка телепорта в зону смерти
    [SerializeField] private Transform respawnPoint;       // Точка возврата в игру
    [SerializeField] private GameObject deathMenuUI;       // Меню смерти (UI Canvas)

    private Animator animator;
    private SimpleEnemyStateManager simpleEnemyStateManager;
    private EnemyStateManager enemyStateManager;

    private bool isPlayer => animator == null && simpleEnemyStateManager == null && enemyStateManager == null;

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

        // Враг
        if (animator != null)
        {
            animator.Play("Death", -1, 0f);
            animator.ResetTrigger("Die");
            animator.SetTrigger("Die");
        }

        if (simpleEnemyStateManager != null)
        {
            simpleEnemyStateManager.Die();
            return;
        }

        if (enemyStateManager != null)
        {
            enemyStateManager.Die();
            return;
        }

        // Игрок
        if (isPlayer && deathZonePoint != null)
        {
            // Телепорт в зону смерти
            transform.position = deathZonePoint.position;
            transform.rotation = deathZonePoint.rotation;

            // Показываем меню смерти
            if (deathMenuUI != null)
                deathMenuUI.SetActive(true);
            // Удаляем врагов в зоне, если есть ссылка
            if (enemyZoneCleaner != null)
            {
                enemyZoneCleaner.ClearEnemiesInZone();
            }

            // Останавливаем игру
            Time.timeScale = 0f;
        }
    }

    void UpdateHologram()
    {
        if (hologramText == null) return;

        hologramText.text = $"HP: {Mathf.Ceil(currentHealth)}";
        hologramText.color = currentHealth <= maxHealth * 0.3f ? Color.red : Color.white;
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        UpdateHologram();
    }

    public float GetHealth() => currentHealth;
    public float GetHealthNormalized() => currentHealth / maxHealth;

    public Transform GetRespawnPoint() => respawnPoint;
}
