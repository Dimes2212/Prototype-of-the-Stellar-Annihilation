//using UnityEngine;

//public class DoorAgroState : DoorBaseState
//{
//    private Transform attackPoint;

//    public override void OnEnter(SimpleEnemyStateManager manager)
//    {
//        Debug.Log("Entering Agro State");

//        // Получаем свободную точку для атаки
//        attackPoint = GetAvailableAttackPoint(manager);

//        if (attackPoint != null)
//        {
//            // Помечаем точку как занятую через метод SetOccupied
//            AttackPoint pointComponent = attackPoint.GetComponent<AttackPoint>();
//            if (pointComponent != null)
//            {
//                pointComponent.SetOccupied(true);
//            }

//            manager.SetSpeed(manager.walkSpeed);
//            manager.SetDestination(attackPoint);
//            manager.animator.SetBool("IsWalking", true);
//        }
//        else
//        {
//            Debug.LogWarning("No available attack points.");
//        }
//    }

//    public override void OnExit(SimpleEnemyStateManager manager)
//    {
//        manager.animator.SetBool("IsWalking", false);

//        if (attackPoint != null)
//        {
//            AttackPoint pointComponent = attackPoint.GetComponent<AttackPoint>();
//            if (pointComponent != null)
//            {
//                pointComponent.SetOccupied(false);
//            }
//        }
//    }

//    public override void OnUpdate(SimpleEnemyStateManager manager)
//    {
//        if (attackPoint == null) return;

//        float distance = Vector3.Distance(manager.transform.position, attackPoint.position);

//        if (distance <= manager.attackDistance)
//        {
//            manager.SwitchState(manager.doorAttackState);
//        }
//    }

//    private Transform GetAvailableAttackPoint(SimpleEnemyStateManager manager)
//    {
//        foreach (Transform point in manager.GetAttackPoints())
//        {
//            if (point == null) continue;

//            AttackPoint ap = point.GetComponent<AttackPoint>();
//            if (ap != null && !ap.IsOccupied)
//            {
//                return point;
//            }
//        }
//        return null;
//    }
//}


using UnityEngine;

public class DoorAgroState : DoorBaseState
{
    private Transform attackPoint;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering Agro State");

        attackPoint = GetAvailableAttackPoint(manager);

        if (attackPoint != null)
        {
            AttackPoint pointComponent = attackPoint.GetComponent<AttackPoint>();
            if (pointComponent != null)
            {
                pointComponent.SetOccupied(true);
            }

            manager.SetSpeed(manager.walkSpeed);
            manager.SetDestination(attackPoint);
            manager.animator.SetBool("IsWalking", true);
        }
        else
        {
            Debug.LogWarning("No available attack points.");
        }
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsWalking", false);

        if (attackPoint != null)
        {
            var pointComponent = attackPoint.GetComponent<AttackPoint>();
            if (pointComponent != null)
                pointComponent.SetOccupied(false);
        }
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (attackPoint == null) return;

        float distance = Vector3.Distance(manager.transform.position, attackPoint.position);
        float remaining = manager.navMeshAgent.remainingDistance;

        if (!manager.navMeshAgent.pathPending &&
            remaining != Mathf.Infinity &&
            manager.navMeshAgent.remainingDistance <= manager.navMeshAgent.stoppingDistance &&
            (!manager.navMeshAgent.hasPath || manager.navMeshAgent.velocity.sqrMagnitude == 0f))
        {
            manager.SwitchState(manager.doorAttackState);
        }
    }

    private Transform GetAvailableAttackPoint(SimpleEnemyStateManager manager)
    {
        foreach (Transform point in manager.GetAttackPoints())
        {
            if (point == null) continue;

            AttackPoint ap = point.GetComponent<AttackPoint>();
            if (ap != null && !ap.IsOccupied)
            {
                return point;
            }
        }
        return null;
    }
}
