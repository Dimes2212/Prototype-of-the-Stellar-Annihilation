using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
        manager.animator.SetBool("IsAttack" , true);

    }
    public override void ExitState(EnemyStateManager manager)
    {
        manager.animator.SetBool("IsAttack", false);
    }
    public override void UpdateState(EnemyStateManager manager)
    {
        Debug.Log("Атакую");
        //if (manager.DistanceToTarget() >= manager.attackDistance)
        //{
        //    manager.SwitchState(manager.agroState);
        //    return;

        //}   
    }
}
