


//using UnityEngine;

//public class DoorDeathState : DoorBaseState
//{
//    public override void OnEnter(SimpleEnemyStateManager manager)
//    {
//        manager.GetComponent<EnemyDeathNotifier>()?.NotifyDeath();
//        Debug.Log("Entering Death State");
//        // Отключаем движение
//        //manager.navMeshAgent.isStopped = true;
//        //manager.navMeshAgent.enabled = false;


//        //manager.animator.ResetTrigger("Die");
//        manager.animator.SetTrigger("Die");



//    }

//    public override void OnUpdate(SimpleEnemyStateManager manager)
//    {

//    }

//    public override void OnExit(SimpleEnemyStateManager manager) { }
//}


using UnityEngine;

public class DoorDeathState : DoorBaseState
{
    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        manager.GetComponent<EnemyDeathNotifier>()?.NotifyDeath();
        Debug.Log("Entering Death State");

        // Анимация смерти
        manager.animator.SetTrigger("Die");

        // Выключаем все коллайдеры
        Collider[] colliders = manager.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Уничтожаем объект через 5 секунд
        Object.Destroy(manager.gameObject, 5f);
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        // Ничего не делаем в состоянии смерти
    }

    public override void OnExit(SimpleEnemyStateManager manager) { }
}
