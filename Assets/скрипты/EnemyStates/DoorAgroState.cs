using UnityEngine;

public class DoorAgroState : DoorBaseState
{
    private Transform doorTarget;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering DoorAgroState");

        doorTarget = manager.GetDoorTarget();

        if (doorTarget == null)
        {
            Debug.LogError("No door target found!");
            return;
        }

        manager.SetSpeed(manager.walkSpeed);  // Получаем walkSpeed через свойство
        manager.animator.SetBool("IsAttack", false);
        manager.animator.SetBool("IsWalking", true);

        manager.SetDestination(doorTarget);
    }

    public override void OnExit(SimpleEnemyStateManager manager) { }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (doorTarget == null) return;

        float distance = Vector3.Distance(manager.transform.position, doorTarget.position);

        if (distance <= manager.attackDistance)  // Получаем attackDistance через свойство
        {
            manager.SwitchState(manager.doorAttackState);
        }
    }
}
