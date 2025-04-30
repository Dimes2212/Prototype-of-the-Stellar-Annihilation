using UnityEngine;

public class Firstwavespawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб врага
    public Transform spawnPoint;   // Точка спавна
    public int enemyCount = 3;      // Количество врагов
    public float spawnRadius = 5f;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned && other.CompareTag("Player")) // Проверяем, вошёл ли игрок
        {
            SpawnEnemies();
            hasSpawned = true; // Чтобы избежать повторного спавна
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)); // Разброс спавна
            Instantiate(enemyPrefab, spawnPoint.position + randomOffset, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius; // Получаем случайную точку в круге
        Vector3 spawnPos = spawnPoint.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        return spawnPos;
    }
}
