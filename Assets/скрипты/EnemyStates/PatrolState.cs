using UnityEngine;

public class PatrolState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        Debug.Log("Entering Patrol");
        manager.SetSpeed(manager.walkSpeed);
        manager.animator.SetBool("IsIdle", false);
        manager.animator.SetBool("IsPatrolling", true);
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsAttack", false);

        if (manager.patrolPoints.Length == 0)
        {
            Debug.LogWarning("Нет точек патруля.");
            return;
        }

        manager.SetDestination(manager.patrolPoints[manager.currentPatrolIndex]);  // Устанавливаем цель на точку патруля
    }

    public override void ExitState(EnemyStateManager manager)
    {
    }

    public override void UpdateState(EnemyStateManager manager)
    {
        float distanceToPlayer = Vector3.Distance(manager.transform.position, manager.GetPlayer().position);

        // Переход в агрессию, если игрок в пределах агро-дистанции
        if (distanceToPlayer < manager.agroDistance)
        {
            manager.SwitchState(manager.agroState);
            return;
        }

        // Если точка патруля достигнута, переключаемся на следующую
        if (!manager.navMeshAgent.pathPending && manager.navMeshAgent.remainingDistance < 0.5f)
        {
            manager.currentPatrolIndex = (manager.currentPatrolIndex + 1) % manager.patrolPoints.Length;
            manager.SetDestination(manager.patrolPoints[manager.currentPatrolIndex]);
        }
    }
}
