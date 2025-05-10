using UnityEngine;

public class DoorAttackState : DoorBaseState
{
    private Transform doorTarget;
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering DoorAttackState");

        doorTarget = manager.GetDoorTarget();

        manager.SetSpeed(0);  // Останавливаемся, когда атакуем
        manager.animator.SetBool("IsWalking", false);
        manager.animator.SetBool("IsAttack", true);

        lastAttackTime = Time.time;
    }

    public override void OnExit(SimpleEnemyStateManager manager) { }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (doorTarget == null) return;

        float distance = Vector3.Distance(manager.transform.position, doorTarget.position);

        // Если враг слишком далеко от двери, вернуться в агрессию
        if (distance > manager.attackDistance)
        {
            manager.SwitchState(manager.doorAgroState);
            return;
        }

        // Атака по кулдауну
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            // Наносим урон двери
            var doorHealth = doorTarget.GetComponent<DoorHealth>();
            if (doorHealth != null)
            {
                doorHealth.TakeDamage(10);  // Параметр 10 — это количество урона
            }

            Debug.Log("Attacking door!");
        }
    }
}
