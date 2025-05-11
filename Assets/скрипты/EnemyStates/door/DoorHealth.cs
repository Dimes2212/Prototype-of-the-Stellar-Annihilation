using UnityEngine;

public class DoorHealth : MonoBehaviour
{
    [SerializeField] private Transform[] attackPoints;  // Точки для атаки

    public bool IsDead()
    {
        // Логика проверки, мертва ли дверь (например, если здоровье ниже 0)
        return false; // Пока что просто всегда возвращаем false
    }

    public void TakeDamage(int damage)
    {
        // Логика получения урона дверью
        Debug.Log($"Door received {damage} damage");
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
