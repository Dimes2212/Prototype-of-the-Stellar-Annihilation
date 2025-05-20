public class AttackState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        if (manager.audioSource != null && manager.attackClip != null)
        {
            manager.audioSource.PlayOneShot(manager.attackClip);
        }

        manager.SetSpeed(0);
        manager.animator.SetBool("IsAttack", true);
    }

    public override void ExitState(EnemyStateManager manager) { }

    public override void UpdateState(EnemyStateManager manager) { }
}
