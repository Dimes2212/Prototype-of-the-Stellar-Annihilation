using UnityEngine;

public class AgroState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        Debug.Log("Entering Agro");
        manager.SetSpeed(manager.walkSpeed);
        manager.animator.SetBool("IsIdle", false);
        manager.animator.SetBool("IsPatrolling", false);
        manager.animator.SetBool("IsAgro", true);
        manager.animator.SetBool("IsAttack", false);



        manager.SetDestination(manager.GetPlayer());  // Цель - игрок
    }

    public override void ExitState(EnemyStateManager manager)
    {
    }

    public override void UpdateState(EnemyStateManager manager)
    {
        float distanceToPlayer = manager.DistanceToTarget();

        // Переход в патруль, если игрок далеко
        if (distanceToPlayer >= manager.agroDistance)
        {
            manager.SwitchState(manager.patrolState);
            return;
        }

        // Переход в атаку, если игрок слишком близко
        if (distanceToPlayer < manager.attackDistance)
        {
            manager.SwitchState(manager.attackState);
            return;
        }

        // Если дистанция меняется в агрессии, продолжаем следовать за игроком
        manager.SetDestination(manager.GetPlayer());
    }
}
