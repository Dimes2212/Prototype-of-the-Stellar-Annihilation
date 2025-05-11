public abstract class DoorBaseState
{
    public abstract void OnEnter(SimpleEnemyStateManager manager);
    public abstract void OnUpdate(SimpleEnemyStateManager manager);
    public abstract void OnExit(SimpleEnemyStateManager manager);
}
