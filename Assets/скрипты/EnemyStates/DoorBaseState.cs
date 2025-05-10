using UnityEngine;

public abstract class DoorBaseState : MonoBehaviour
{
    // Метод вызывается при входе в состояние
    public abstract void OnEnter(SimpleEnemyStateManager manager);

    // Метод вызывается при выходе из состояния
    public abstract void OnExit(SimpleEnemyStateManager manager);

    // Метод обновляется каждый кадр
    public abstract void OnUpdate(SimpleEnemyStateManager manager);
}
