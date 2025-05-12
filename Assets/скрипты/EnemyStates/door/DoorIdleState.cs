using UnityEngine;

public class DoorIdleState : DoorBaseState
{
    private float checkInterval = 1f;
    private float checkTimer = 0f;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsIdle", true);
        manager.navMeshAgent.isStopped = true;
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsIdle", false);
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        checkTimer += Time.deltaTime;

        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;

            Collider attackZone = manager.GetNearestAttackZone();
            if (attackZone != null)
            {
                manager.SwitchState(manager.doorAgroState);
                manager.SetAttackDestination(attackZone);
            }
        }
    }
}
