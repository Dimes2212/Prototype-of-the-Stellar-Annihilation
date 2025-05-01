using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;              // Сколько урона наносить
    public string targetTag = "Player";           // Кого атакуем
    public bool canDealDamage = true;             // Можно ли наносить урон прямо сейчас
    public float damageCooldown = 1f;             // Задержка между ударами

    private float damageTimer = 0f;

    void Update()
    {
        // Считаем кулдаун
        if (!canDealDamage)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageCooldown)
            {
                canDealDamage = true;
                damageTimer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage) return;
        if (!other.CompareTag(targetTag)) return;

        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
            canDealDamage = false; // Не спамим уроном каждый кадр
        }
    }
}
