using UnityEngine;

public class PatrolState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        Debug.Log("Entering Patrol");
        manager.SetSpeed(manager.walkSpeed);
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsAttack", false);

        if (manager.patrolPoints.Length == 0)
        {
            Debug.LogWarning("Нет точек патруля у врага.");
            return;
        }

        // Начинаем с первой точки
        manager.currentPatrolIndex = 0;
        manager.SetDestination(manager.patrolPoints[manager.currentPatrolIndex]);
    }

    public override void ExitState(EnemyStateManager manager)
    {
        // Здесь можно выключить анимацию патруля, если есть
    }

    public override void UpdateState(EnemyStateManager manager)
    {
        float distanceToPlayer = Vector3.Distance(manager.transform.position, manager.GetPlayer().position);

        // Если игрок в зоне агро — переключаемся в агро
        if (distanceToPlayer < manager.agroDistance)
        {
            manager.SwitchState(manager.agroState);
            return;
        }

        // Если достигнута текущая точка патруля — двигаемся к следующей
        if (!manager.navMeshAgent.pathPending && manager.navMeshAgent.remainingDistance < 0.5f)
        {
            manager.currentPatrolIndex = (manager.currentPatrolIndex + 1) % manager.patrolPoints.Length;
            manager.SetDestination(manager.patrolPoints[manager.currentPatrolIndex]);
        }
    }
}
