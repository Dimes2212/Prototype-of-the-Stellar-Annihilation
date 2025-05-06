using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
        manager.animator.SetBool("IsIdle", false);
        manager.animator.SetBool("IsPatrolling", false);
        manager.animator.SetBool("IsAgro", true);
        manager.animator.SetBool("IsAttack", true);
    }

    public override void ExitState(EnemyStateManager manager)
    {
    }

    public override void UpdateState(EnemyStateManager manager)
    {
        float distance = manager.DistanceToTarget();

        
        if (distance > manager.attackDistance)
        {
            manager.SwitchState(manager.agroState);
            return;
        }

        
    }
}
