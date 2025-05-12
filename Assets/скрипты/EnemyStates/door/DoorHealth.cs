using UnityEngine;

public class DoorHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject destructionEffect;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip destroySound;

    private int currentHealth;
    private bool isDead = false;
    private AudioSource audioSource;

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        PlaySound(hitSound);

        Debug.Log($"Door took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        PlaySound(destroySound);

        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, transform.rotation);
        }

        // Отключаем коллайдер и рендерер
        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        var renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;

        // Уничтожаем объект через 2 секунды (после проигрывания звука)
        Destroy(gameObject, 2f);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Для восстановления здоровья (если нужно)
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}