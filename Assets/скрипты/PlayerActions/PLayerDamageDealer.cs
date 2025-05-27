using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;
    public string targetTag = "Enemy";
    public float damageCooldown = 0.5f;

    private bool canDealDamage = true;
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (other.CompareTag("Enemy")&& health != null)
        {
            // Загрузка префаба по имени (без расширения!)
            GameObject bloodEffectPrefab = Resources.Load<GameObject>("BloodSprayFX");

            if (bloodEffectPrefab != null)
            {
                Instantiate(bloodEffectPrefab, other.transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Префаб BloodEffect не найден в папке Resources!");
            }
        }
        if (!canDealDamage || !other.CompareTag(targetTag)) return;

        if (health != null)
        {

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
