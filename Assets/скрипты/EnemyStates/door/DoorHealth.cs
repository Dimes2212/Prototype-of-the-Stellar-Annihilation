using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DoorHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public UnityEvent onDamage;
    public UnityEvent onDeath;
    public TextMeshProUGUI hologramText;
    [SerializeField] private GameObject destructionEffect;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource destroySound;

    private AudioSource audioSource;
    private bool isDead = false;
    private float lastHitTime = -3f; 
    private const float HIT_SOUND_COOLDOWN = 3f; 
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

        if (Time.time - lastHitTime >= HIT_SOUND_COOLDOWN)
        {
            hitSound.Play();
            lastHitTime = Time.time;
        }

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
        destroySound.Play();

        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, transform.rotation);
        }

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

        hologramText.text = $"{Mathf.Ceil(currentHealth)} HP";
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