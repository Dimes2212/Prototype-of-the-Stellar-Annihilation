using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        if (health != null)
        {
            health.onDeath.AddListener(OnEnemyDeath);
        }
    }

    // Этот метод теперь можно вызывать вручную из других скриптов
    public void NotifyDeath()
    {
        OnEnemyDeath();
    }

    private void OnEnemyDeath()
    {
        GameManager.Instance?.AddKill();
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.onDeath.RemoveListener(OnEnemyDeath);
        }
    }
}