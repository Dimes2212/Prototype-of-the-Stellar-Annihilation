using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnAreaMin;
    public Transform spawnAreaMax;
    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 5;
    public float timeBetweenSpawns = 1f;
    private bool isSpawning = false;
    void Start()
    {
        StartCoroutine(SpawnWave());
    }
    IEnumerator SpawnWave()
    {
        while (true)
        {
            if (!isSpawning)
            {
                isSpawning = true;
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
                yield return new WaitForSeconds(timeBetweenWaves);

                isSpawning = false;
            }
            yield return null;
        }
    }
    void SpawnEnemy()
    {
        Vector3 spawnPos;
        spawnPos = GenerateRandomPosition();
        if (IsPositionFree(spawnPos))
        {
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            SpawnEnemy();
        }
    }
    Vector3 GenerateRandomPosition()
    {
        Vector3 min = spawnAreaMin.position;
        Vector3 max = spawnAreaMax.position;

        return new Vector3(
            Random.Range(min.x, max.x),
            min.y,
            Random.Range(min.z, max.z)
        );
    }
    bool IsPositionFree(Vector3 pos, float radius = 1.5f)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
        return hitColliders.Length == 0;
    }
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
