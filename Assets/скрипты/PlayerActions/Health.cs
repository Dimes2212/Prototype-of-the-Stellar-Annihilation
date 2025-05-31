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
    [SerializeField] private Transform deathZonePoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject deathMenuUI;
    [SerializeField] private AudioSource playerHurtSound;
    [SerializeField, Range(0f, 1f)] private float playerHurtVolume = 1f;
    [SerializeField] private AudioSource EnemyAttack;
    [SerializeField, Range(0f, 1f)] private float EnemyAttackVolume = 1f;
    [SerializeField] private AudioSource enemyHurtSound;
    [SerializeField] private AudioSource enemyDieSound;
    [SerializeField, Range(0f, 1f)] private float enemyHurtVolume = 1f;

    private Animator animator;
    private SimpleEnemyStateManager simpleEnemyStateManager;
    private EnemyStateManager enemyStateManager;
    private bool isInitialized = false;

    private bool isPlayer => animator == null && simpleEnemyStateManager == null && enemyStateManager == null;
    private bool isEnemy => gameObject.CompareTag("Enemy");

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        simpleEnemyStateManager = GetComponent<SimpleEnemyStateManager>();
        enemyStateManager = GetComponent<EnemyStateManager>();

        if (playerHurtSound != null)
        {
            playerHurtSound.playOnAwake = false;
            playerHurtSound.volume = playerHurtVolume;
        }

        if (enemyHurtSound != null)
        {
            enemyHurtSound.playOnAwake = false;
            enemyHurtSound.volume = enemyHurtVolume;
        }

        UpdateHologram();
        isInitialized = true;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0 || !isInitialized) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        onDamage?.Invoke();
        UpdateHologram();

        if (isPlayer && playerHurtSound != null && EnemyAttack != null)
        {
            EnemyAttack.Play();
            playerHurtSound.Play();
        }
        else if (isEnemy && enemyHurtSound != null)
        {
            enemyHurtSound.Play();
        }

        if (currentHealth <= 0)
            Die();
    }

    public void SetPlayerHurtVolume(float volume)
    {
        playerHurtVolume = Mathf.Clamp01(volume);
        if (playerHurtSound != null)
            playerHurtSound.volume = playerHurtVolume;
    }

    public void SetEnemyHurtVolume(float volume)
    {
        enemyHurtVolume = Mathf.Clamp01(volume);
        if (enemyHurtSound != null)
            enemyHurtSound.volume = enemyHurtVolume;
    }

    public void Heal(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHologram();
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    void Die()
    {
        onDeath?.Invoke();

        // Начисляем деньги за убийство врага
        if ((simpleEnemyStateManager != null || enemyStateManager != null) && rewardOnDeath > 0)
        {
            PlayerCurrency playerCurrency = FindObjectOfType<PlayerCurrency>();
            if (playerCurrency != null)
            {
                playerCurrency.AddCurrency(rewardOnDeath);
            }
        }

        if (animator != null)
        {
            animator.Play("Death", -1, 0f);
            animator.ResetTrigger("Die");
            animator.SetTrigger("Die");
        }

        if (simpleEnemyStateManager != null)
        {
            enemyDieSound.Play();
            simpleEnemyStateManager.Die();
            return;
        }

        if (enemyStateManager != null)
        {
            Collider[] allColliders = GetComponents<Collider>();
            foreach (Collider col in allColliders)
            {
                col.enabled = false;
            }
            enemyDieSound.Play();
            enemyStateManager.Die();
            return;
        }

        if (isPlayer && deathZonePoint != null)
        {
            transform.position = deathZonePoint.position;
            transform.rotation = deathZonePoint.rotation;

            if (deathMenuUI != null)
                deathMenuUI.SetActive(true);

            if (enemyZoneCleaner != null)
                enemyZoneCleaner.ClearEnemiesInZone();

            Time.timeScale = 0f;
        }
    }

    void UpdateHologram()
    {
        if (hologramText == null) return;

        hologramText.text = $" {Mathf.Ceil(currentHealth)} HP";
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