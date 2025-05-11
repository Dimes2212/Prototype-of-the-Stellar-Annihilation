using UnityEngine;

public class DoorIdleState : DoorBaseState
{
    private float checkInterval = 1f;  // интервал между проверками
    private float checkTimer = 0f;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering IdleState");

        // Убедитесь, что manager не равен null
        if (manager.GetDoorTarget() == null)
        {
            Debug.LogError("Door target is not assigned.");
            return;
        }

        manager.animator.SetBool("IsIdle", true);  // Анимация ожидания
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
            Transform availablePoint = GetAvailableAttackPoint(manager);

            if (availablePoint != null)
            {
                Debug.Log("Found available attack point, switching to AgroState.");
                manager.SwitchState(manager.doorAgroState);
                manager.SetDestination(availablePoint);  // Назначаем цель для движения
            }
            else
            {
                Debug.Log("No available attack point found.");
            }
        }
    }

    private Transform GetAvailableAttackPoint(SimpleEnemyStateManager manager)
    {
        foreach (Transform point in manager.GetAttackPoints())
        {
            AttackPoint attackPoint = point.GetComponent<AttackPoint>();
            if (attackPoint != null && !attackPoint.IsOccupied)
            {
                Debug.Log("Available attack point found: " + point.name);  // Логируем доступную точку
                return point;
            }
        }
        return null;  // Если все точки заняты, возвращаем null
    }
}
