//using UnityEngine;

//public class DoorHealth : MonoBehaviour
//{
//    [SerializeField] private int maxHealth = 100;
//    [SerializeField] private GameObject destructionEffect;
//    [SerializeField] private AudioClip hitSound;
//    [SerializeField] private AudioClip destroySound;

//    private int currentHealth;
//    private bool isDead = false;
//    private AudioSource audioSource;

//    public int CurrentHealth => currentHealth;
//    public bool IsDead => isDead;

//    private void Awake()
//    {
//        currentHealth = maxHealth;
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null)
//        {
//            audioSource = gameObject.AddComponent<AudioSource>();
//        }
//    }

//    public void TakeDamage(int damage)
//    {
//        if (isDead) return;

//        currentHealth -= damage;
//        PlaySound(hitSound);

//        Debug.Log($"Door took {damage} damage. Remaining health: {currentHealth}");

//        if (currentHealth <= 0)
//        {
//            Die();
//        }
//    }

//    private void Die()
//    {
//        isDead = true;
//        PlaySound(destroySound);

//        if (destructionEffect != null)
//        {
//            Instantiate(destructionEffect, transform.position, transform.rotation);
//        }

//        // Отключаем коллайдер и рендерер
//        var collider = GetComponent<Collider>();
//        if (collider != null) collider.enabled = false;

//        var renderer = GetComponent<Renderer>();
//        if (renderer != null) renderer.enabled = false;

//        // Уничтожаем объект через 2 секунды (после проигрывания звука)
//        Destroy(gameObject, 2f);
//    }

//    private void PlaySound(AudioClip clip)
//    {
//        if (clip != null && audioSource != null)
//        {
//            audioSource.PlayOneShot(clip);
//        }
//    }

//    // Для восстановления здоровья (если нужно)
//    public void Heal(int amount)
//    {
//        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
//    }
//}


using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DoorHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Events")]
    public UnityEvent onDamage;
    public UnityEvent onDeath;

    [Header("UI")]
    public TextMeshProUGUI hologramText;

    [Header("Effects")]
    [SerializeField] private GameObject destructionEffect;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip destroySound;

    private AudioSource audioSource;
    private bool isDead = false;

    public float CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        UpdateHologram();
    }

    public void TakeDamage(float damage)
    {
        if (isDead || currentHealth <= 0) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0f);
        onDamage?.Invoke();
        PlaySound(hitSound);
        UpdateHologram();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        onDeath?.Invoke();
        PlaySound(destroySound);

        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, transform.rotation);
        }

        // Отключаем визуальные компоненты и коллайдер
        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        var renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;

        UpdateHologram();
        Destroy(gameObject, 2f);
    }

    void UpdateHologram()
    {
        if (hologramText == null) return;

        hologramText.text = $"Door HP: {Mathf.Ceil(currentHealth)}";
        hologramText.color = currentHealth <= maxHealth * 0.3f ? Color.red : Color.white;
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHologram();
    }

    public float GetHealthNormalized() => currentHealth / maxHealth;

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}