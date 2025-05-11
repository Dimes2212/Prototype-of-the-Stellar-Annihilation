using UnityEngine;

public class DoorAgroState : DoorBaseState
{
    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering AgroState");

        // Убедимся, что у нас есть цель
        if (manager.GetDoorTarget() == null)
        {
            Debug.LogError("No door target set.");
            return;
        }

        // Устанавливаем точку назначения на первую доступную точку
        Transform attackPoint = manager.GetAttackPoint();
        if (attackPoint != null)
        {
            manager.SetDestination(attackPoint);
        }
        else
        {
            Debug.LogError("No available attack point found.");
        }
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        Debug.Log("Exiting AgroState");
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (manager.DistanceToTarget() <= manager.attackDistance)
        {
            manager.SwitchState(manager.doorAttackState);
        }
    }
}
