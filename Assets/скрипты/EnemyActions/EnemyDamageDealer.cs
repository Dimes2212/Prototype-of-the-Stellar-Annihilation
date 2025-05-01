using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    public float damageAmount = 15f;
    public string targetTag = "Player";
    public float damageCooldown = 1f;

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
