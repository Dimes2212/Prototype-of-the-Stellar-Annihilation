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
            manager.SetAttackDestination(targetZone);
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
