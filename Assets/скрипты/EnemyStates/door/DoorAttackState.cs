using UnityEngine;

public class DoorAttackState : DoorBaseState
{
    private Transform doorTarget;
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering AttackState");

        doorTarget = manager.GetDoorTarget();

        manager.animator.SetBool("IsWalking", false);  // Останавливаем анимацию ходьбы
        manager.animator.SetBool("IsAttack", true);  // Включаем анимацию атаки

        lastAttackTime = Time.time;
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsAttack", false);  // Отключаем анимацию атаки при выходе
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (doorTarget == null) return;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            // Наносим урон двери
            var doorHealth = doorTarget.GetComponent<DoorHealth>();
            if (doorHealth != null && !doorHealth.IsDead())
            {
                doorHealth.TakeDamage(10);  // Параметр 10 — это количество урона
                Debug.Log("Attacking door at " + doorTarget.position);
            }
        }
    }
}
