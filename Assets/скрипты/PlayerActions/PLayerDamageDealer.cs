using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;
    public string targetTag = "Enemy";
    public float damageCooldown = 0.5f;

    private bool canDealDamage = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage || !other.CompareTag(targetTag)) return;

        Health health = other.GetComponent<Health>();
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
