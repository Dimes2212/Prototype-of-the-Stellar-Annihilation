using UnityEngine;

public class DoorDeathState : DoorBaseState
{
    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsDead", true);
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        // Ничего не делаем, враг мертв
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        // Ничего
    }
}
