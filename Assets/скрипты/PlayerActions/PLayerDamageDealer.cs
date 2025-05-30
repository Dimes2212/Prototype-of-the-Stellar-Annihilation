using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 10f;
    public string targetTag = "Enemy";
    public float damageCooldown = 0.5f;

    [Header("Effects")]
    [SerializeField] private AudioSource weaponSound;
    [SerializeField] private GameObject bloodEffectPrefab;
    [SerializeField] private float effectDuration = 1f;
    [SerializeField] private bool spawnEffectOnWeapon = false; // Для меча = true, для пули = false

    private bool canDealDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        // -- Эффект крови --
        if (bloodEffectPrefab != null)
        {
            Vector3 spawnPosition = spawnEffectOnWeapon
                ? transform.position                          // Для меча: на оружии
                : other.ClosestPoint(transform.position);     // Для пули: точка контакта

            GameObject effect = Instantiate(
                bloodEffectPrefab,
                spawnPosition,
                Quaternion.LookRotation(transform.forward)    // Направление эффекта
            );

            Destroy(effect, effectDuration);
        }

        // -- Урон --
        Health health = other.GetComponent<Health>();
        if (!canDealDamage || health == null) return;
        if (health != null)
        {
            weaponSound.Play();
            health.TakeDamage(damageAmount);
            StartCoroutine(DamageCooldown());
        }
    }

    private System.Collections.IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDealDamage = true;
    }
}