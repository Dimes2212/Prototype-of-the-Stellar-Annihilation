using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
        manager.animator.SetBool("IsIdle", true);
        manager.animator.SetBool("IsPatrolling", false);
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsAttack", false);
    }

    public override void ExitState(EnemyStateManager manager)
    {
        Debug.Log("Exited Idle");
    }

    public override void UpdateState(EnemyStateManager manager)
    {
        float distanceToPlayer = manager.DistanceToTarget();

        // Переход в агрессию, если игрок близко
        if (distanceToPlayer < manager.agroDistance)
        {
            manager.SwitchState(manager.agroState);
            return;
        }

        // Переход в патруль, если игрок далеко
        if (distanceToPlayer >= manager.agroDistance)
        {
            manager.SwitchState(manager.patrolState);
            return;
        }
    }
}
