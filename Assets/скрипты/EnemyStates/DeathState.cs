using UnityEngine;

public class DeathState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        Debug.Log("Entering Death State");

        
        manager.animator.SetBool("isDead", true);
        manager.animator.SetBool("IsIdle", false);
        manager.animator.SetBool("IsPatrolling", false);
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsAttack", false);


        
        //manager.GetComponent<Health>().onDeath.AddListener(() => Destroy(manager.gameObject));  
    }

    public override void ExitState(EnemyStateManager manager)
    {
        
    }

    public override void UpdateState(EnemyStateManager manager)
    {
        
    }
}
