using UnityEngine;
using System.Collections.Generic;

public class EnemyZoneCleaner : MonoBehaviour
{
    public string enemyTag = "Enemy";

    private readonly List<GameObject> enemiesInZone = new();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag) && !enemiesInZone.Contains(other.gameObject))
        {
            enemiesInZone.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            enemiesInZone.Remove(other.gameObject);
        }
    }

    public void ClearEnemiesInZone()
    {
        foreach (GameObject enemy in enemiesInZone)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        enemiesInZone.Clear();
    }
}
