using UnityEngine;

public class DoorAgroState : DoorBaseState
{
    private Collider targetZone;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        targetZone = manager.GetNearestAttackZone();

        if (targetZone != null)
        {
            manager.SetSpeed(manager.walkSpeed);

            // Новое: использование случайной точки внутри зоны
            if (targetZone.TryGetComponent<AttackPoint>(out var zone))
            {
                manager.SetDestination(zone.GetRandomPoint());
            }
            else
            {
                manager.SetDestination(targetZone.transform.position);
            }

            manager.animator.SetBool("IsWalking", true);
        }
        else
        {
            manager.SwitchState(manager.doorIdleState);
        }
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (targetZone == null)
        {
            manager.SwitchState(manager.doorIdleState);
            return;
        }

        // Изменено: использование DistanceToCollider вместо Vector3.Distance
        float distance = manager.DistanceToCollider(targetZone);

        if (distance <= manager.attackDistance)
        {
            manager.SwitchState(manager.doorAttackState);
        }
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsWalking", false);
        manager.navMeshAgent.isStopped = true;
    }
}