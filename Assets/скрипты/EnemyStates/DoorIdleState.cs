using UnityEngine;

public class DoorIdleState : DoorBaseState
{
    private Transform[] attackPoints;
    private float checkInterval = 1f;  // интервал между проверками
    private float checkTimer = 0f;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering IdleState");

        attackPoints = manager.GetDoorTarget().GetComponentsInChildren<Transform>();
        attackPoints = System.Array.FindAll(attackPoints, point => point.GetComponent<Collider>() != null);

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
                // Если точка найдена, переходим в агрессию
                manager.SwitchState(manager.doorAgroState);
            }
        }
    }

    private Transform GetAvailableAttackPoint(SimpleEnemyStateManager manager)
    {
        foreach (Transform point in attackPoints)
        {
            // Пропускаем саму дверь и уже занятые точки
            if (point == null || point.gameObject == manager.GetDoorTarget().gameObject) continue;

            AttackPoint attackPoint = point.GetComponent<AttackPoint>();
            if (attackPoint != null && !attackPoint.IsOccupied)
            {
                return point;
            }
        }
        return null;  // Если все точки заняты, возвращаем null
    }
}
