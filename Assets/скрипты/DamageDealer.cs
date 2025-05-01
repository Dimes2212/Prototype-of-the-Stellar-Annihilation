using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;
    public string targetTag = "Player";
    public float damageCooldown = 1f;

    private bool canDealDamage = true;

    void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage) return;
        if (!other.CompareTag(targetTag)) return;

        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
            StartCoroutine(DamageCooldown());
        }
    }

    System.Collections.IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDealDamage = true;
    }
}
