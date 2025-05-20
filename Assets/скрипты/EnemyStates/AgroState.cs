public class AgroState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        

        if (manager.audioSource != null && manager.agroClip != null)
        {
            manager.audioSource.PlayOneShot(manager.agroClip);
        }

        manager.SetSpeed(manager.walkSpeed);
        manager.animator.SetBool("IsIdle", false);
        manager.animator.SetBool("IsPatrolling", false);
        manager.animator.SetBool("IsAgro", true);
        manager.animator.SetBool("IsAttack", false);

        manager.SetDestination(manager.GetPlayer());
    }

    public override void ExitState(EnemyStateManager manager) { }

    public override void UpdateState(EnemyStateManager manager)
    {
        float distanceToPlayer = manager.DistanceToTarget();

        if (distanceToPlayer >= manager.agroDistance)
        {
            manager.SwitchState(manager.patrolState);
            return;
        }

        if (distanceToPlayer < manager.attackDistance)
        {
            manager.SwitchState(manager.attackState);
            return;
        }

        manager.SetDestination(manager.GetPlayer());
    }
}
