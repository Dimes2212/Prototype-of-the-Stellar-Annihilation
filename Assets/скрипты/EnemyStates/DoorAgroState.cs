using UnityEngine;

public class DoorAgroState : DoorBaseState
{
    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        // Получаем доступную точку для атаки
        var availableAttackPoint = manager.GetAvailableAttackPoint();

        if (availableAttackPoint != null)
        {
            // Устанавливаем точку атаки
            manager.SetDestination(availableAttackPoint);
            availableAttackPoint.GetComponent<AttackPoint>().SetOccupied(true);  // Занимаем точку
        }
        else
        {
            // Все точки заняты, переходим в состояние ожидания
            manager.SwitchState(manager.doorIdleState);
        }
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        // Освобождаем точку после выхода из состояния
        var attackPoint = manager.GetAvailableAttackPoint();
        if (attackPoint != null)
        {
            attackPoint.GetComponent<AttackPoint>().SetOccupied(false);
        }
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (manager.DistanceToTarget() <= manager.AttackDistance)
        {
            manager.SwitchState(manager.doorAttackState);
        }
    }
}
