using UnityEngine;

public class DoorDeathState : DoorBaseState
{
    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        manager.SetSpeed(0);
        Debug.Log("Enemy died.");
    }

    public override void OnExit(SimpleEnemyStateManager manager) { }

    public override void OnUpdate(SimpleEnemyStateManager manager) { }
}
