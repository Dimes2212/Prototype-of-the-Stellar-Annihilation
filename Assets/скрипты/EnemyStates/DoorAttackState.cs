using UnityEngine;

public class DoorAttackState : DoorBaseState
{
    private Transform doorTarget;
    private Transform attackPoint;
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering DoorAttackState");

        doorTarget = manager.GetDoorTarget();
        attackPoint = manager.GetAvailableAttackPoint();

        manager.SetSpeed(0);  // Останавливаемся, когда атакуем
        manager.animator.SetBool("IsAttack", true);

        lastAttackTime = Time.time;
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsAttack", false);
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (doorTarget == null || attackPoint == null) return;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            // Наносим урон двери
            var doorHealth = doorTarget.GetComponent<DoorHealth>();
            if (doorHealth != null && !doorHealth.IsDead())
            {
                doorHealth.TakeDamage(10);  // Параметр 10 — это количество урона
            }

            Debug.Log("Attacking door at " + attackPoint.position);
        }
    }
}
