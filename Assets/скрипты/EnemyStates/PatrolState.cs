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

        manager.SetDestination(manager.patrolPoints[manager.currentPatrolIndex]);  
    }

    public override void ExitState(EnemyStateManager manager)
    {
    }

    public override void UpdateState(EnemyStateManager manager)
    {
        if (!manager.navMeshAgent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent не находится на NavMesh.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(manager.transform.position, manager.GetPlayer().position);

        if (distanceToPlayer < manager.agroDistance)
        {
            manager.SwitchState(manager.agroState);
            return;
        }

        if (!manager.navMeshAgent.pathPending && manager.navMeshAgent.remainingDistance < 0.5f)
        {
            manager.currentPatrolIndex = (manager.currentPatrolIndex + 1) % manager.patrolPoints.Length;
            manager.SetDestination(manager.patrolPoints[manager.currentPatrolIndex]);
        }
    }

}
