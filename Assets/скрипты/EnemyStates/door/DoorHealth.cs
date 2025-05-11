using UnityEngine;

public class DoorHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private Transform[] attackPoints;  // Точки для атаки

    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Door received {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("The door has been destroyed!");

        // Можно добавить анимацию разрушения двери или другие эффекты
        // Например, добавим задержку перед уничтожением объекта

        // Удаляем объект через 2 секунды (можно заменить на другой эффект, если требуется)
        Destroy(gameObject, 2f);
    }

    public Transform GetAvailableAttackPoint()
    {
        foreach (var point in attackPoints)
        {
            AttackPoint attackPoint = point.GetComponent<AttackPoint>();
            if (attackPoint != null && !attackPoint.IsOccupied)
            {
                return point;
            }
        }
        return null;  // Если все точки заняты, возвращаем null
    }
}
