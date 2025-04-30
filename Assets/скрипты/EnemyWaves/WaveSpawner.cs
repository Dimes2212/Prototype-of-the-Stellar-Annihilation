using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    // Префаб врага
    public GameObject enemyPrefab;

    // Объекты, задающие границы зоны спавна
    public Transform spawnAreaMin;
    public Transform spawnAreaMax;

    // Время между волнами спавна
    public float timeBetweenWaves = 5f;

    // Количество врагов в каждой волне
    public int enemiesPerWave = 5;

    // Задержка между спавнами отдельных врагов
    public float timeBetweenSpawns = 1f;

    // Флаг, чтобы не спавнить врагов до окончания предыдущей волны
    private bool isSpawning = false;

    // Стартуем процесс спавна волны
    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    // Корутина для спавна волны
    IEnumerator SpawnWave()
    {
        while (true)
        {
            if (!isSpawning)
            {
                isSpawning = true;

                // Спавним врагов в течение определенного времени
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }

                // Ждем перед началом следующей волны
                yield return new WaitForSeconds(timeBetweenWaves);

                isSpawning = false;
            }

            // Задержка между волнами
            yield return null;
        }
    }

    // Функция для спавна врага в случайной позиции внутри зоны
    void SpawnEnemy()
    {
        Vector3 spawnPos;

        // Генерируем случайную позицию
        spawnPos = GenerateRandomPosition();

        // Если позиция свободна, спавним врага
        if (IsPositionFree(spawnPos))
        {
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            // Если позиция занята, пробуем заново
            SpawnEnemy();
        }
    }

    // Генерация случайной позиции в зоне спавна
    Vector3 GenerateRandomPosition()
    {
        Vector3 min = spawnAreaMin.position;
        Vector3 max = spawnAreaMax.position;

        return new Vector3(
            Random.Range(min.x, max.x),
            min.y, // Задаем фиксированную высоту
            Random.Range(min.z, max.z)
        );
    }

    // Проверка на занятость позиции (есть ли коллайдеры в радиусе)
    bool IsPositionFree(Vector3 pos, float radius = 1.5f)
    {
        // Проверка на занятость с использованием OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
        return hitColliders.Length == 0;
    }

    // Для отладки в редакторе (отображение зоны спавна)
    void OnDrawGizmos()
    {
        if (spawnAreaMin != null && spawnAreaMax != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((spawnAreaMin.position + spawnAreaMax.position) / 2,
                new Vector3(
                    Mathf.Abs(spawnAreaMax.position.x - spawnAreaMin.position.x),
                    1f,
                    Mathf.Abs(spawnAreaMax.position.z - spawnAreaMin.position.z)
                ));
        }
    }
}
