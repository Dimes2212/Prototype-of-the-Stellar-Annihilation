using UnityEngine;

public class DeathState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        Debug.Log("Entering Death State");

        manager.GetComponent<EnemyDeathNotifier>()?.NotifyDeath();

        if (manager.audioSource != null && manager.deathClip != null)
        {
            manager.audioSource.PlayOneShot(manager.deathClip);
        }

        manager.animator.SetBool("isDead", true);
        manager.animator.SetBool("IsIdle", false);
        manager.animator.SetBool("IsPatrolling", false);
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsAttack", false);
    }

    public override void ExitState(EnemyStateManager manager) { }

    public override void UpdateState(EnemyStateManager manager) { }
}
