


using UnityEngine;

public class DoorDeathState : DoorBaseState
{
    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        GameStatsManager.Instance?.EnemyKilled();
        Debug.Log("Entering Death State");
        // Отключаем движение
        //manager.navMeshAgent.isStopped = true;
        //manager.navMeshAgent.enabled = false;

      
        //manager.animator.ResetTrigger("Die");
        manager.animator.SetTrigger("Die");
        

       
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
       
    }

    public override void OnExit(SimpleEnemyStateManager manager) { }
}
