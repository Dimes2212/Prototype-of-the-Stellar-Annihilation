using UnityEngine;

public class DoorDeathState : DoorBaseState
{
    private float deathTimer;
    private bool animationTriggered;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        deathTimer = 0f;
        animationTriggered = false;

        // 1. Отключаем все мешающие компоненты
        manager.navMeshAgent.isStopped = true;
        manager.navMeshAgent.enabled = false;

        // 2. Запускаем анимацию смерти
        if (manager.animator != null)
        {
            manager.animator.SetTrigger("Die");
            animationTriggered = true;
        }

        // 3. Отключаем физику и коллайдеры
        foreach (var collider in manager.GetComponents<Collider>())
        {
            collider.enabled = false;
        }
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (!animationTriggered)
        {
            // Если анимация не запустилась, уничтожаем сразу
            Object.Destroy(manager.gameObject);
            return;
        }

        deathTimer += Time.deltaTime;

        // Ждем минимум 3 секунды перед уничтожением
        if (deathTimer >= 3f)
        {
            // Дополнительная проверка: если анимация закончилась
            if (manager.animator.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                manager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                Object.Destroy(manager.gameObject);
            }
            else if (!manager.animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                Object.Destroy(manager.gameObject);
            }
        }
    }

    public override void OnExit(SimpleEnemyStateManager manager) { }
}