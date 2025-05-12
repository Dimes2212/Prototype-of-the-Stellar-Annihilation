using UnityEngine;

public class DoorAttackState : DoorBaseState
{
    private float lastAttackTime;
    private Collider attackZone;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        attackZone = manager.GetNearestAttackZone();
        manager.animator.SetBool("IsWalking", false);
        manager.animator.SetBool("IsAttack", true);
        manager.navMeshAgent.isStopped = true;
        lastAttackTime = Time.time;

        // Новое: проигрывание эффектов при входе в состояние
        if (attackZone != null && attackZone.TryGetComponent<AttackPoint>(out var zone))
        {
            zone.PlayHitEffects();
        }
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (Time.time - lastAttackTime >= manager.attackCooldown)
        {
            lastAttackTime = Time.time;
            manager.AttackDoor();

            // Новое: проигрывание эффектов при каждой атаке
            if (attackZone != null && attackZone.TryGetComponent<AttackPoint>(out var zone))
            {
                zone.PlayHitEffects();
            }
        }

        // Изменено: проверка расстояния до коллайдера
        if (attackZone == null || manager.DistanceToCollider(attackZone) > manager.attackDistance * 1.5f)
        {
            manager.SwitchState(manager.doorIdleState);
        }
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsAttack", false);
    }
}