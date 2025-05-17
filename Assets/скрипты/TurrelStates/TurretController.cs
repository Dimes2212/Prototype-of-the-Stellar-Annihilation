using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform firePoint;                  
    public GameObject bulletPrefab;              
    public float shootInterval = 1f;             
    public float detectionRange = 15f;           
    public string targetTag = "Enemy";           
    public Transform rotatingPart;               
    public LayerMask ignoreVisionLayers;         

    private float shootTimer = 0f;
    private Transform currentTarget;

    void Update()
    {
        FindTarget();

        if (currentTarget != null)
        {
            RotateTowardsTarget();

            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;
        currentTarget = null;

        foreach (GameObject enemy in enemies)
        {
            Transform targetPoint = enemy.transform.Find("TurretTargetPoint");
            if (targetPoint == null) continue;

            float distance = Vector3.Distance(transform.position, targetPoint.position);
            if (distance > detectionRange) continue;

            Vector3 direction = targetPoint.position - firePoint.position;

            if (Physics.Raycast(firePoint.position, direction.normalized, out RaycastHit hit, detectionRange, ~ignoreVisionLayers))
            {
                if (hit.transform == targetPoint || hit.transform.IsChildOf(enemy.transform))
                {
                    currentTarget = targetPoint;
                    closestDistance = distance;
                }
            }
        }
    }

    void RotateTowardsTarget()
    {
        if (rotatingPart == null || currentTarget == null) return;

        // Направление от башни к цели
        Vector3 direction = currentTarget.position - rotatingPart.position;
        direction.y = 0f; // Чтобы крутилось только по горизонтали

        if (direction.sqrMagnitude > 0.01f)
        {
            // Крутить по X-вектору, но с использованием Z forward
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
            // Преобразуем так, чтобы "вперёд" считалась ось X (поворачиваем -90 градусов вокруг Y)
            lookRotation *= Quaternion.Euler(0, -90f, 0);
            rotatingPart.rotation = Quaternion.Slerp(rotatingPart.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null && currentTarget != null)
        {
            Vector3 shootDirection = (currentTarget.position - firePoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(shootDirection);

            // Пули должны лететь по Z вперёд, независимо от ориентации дуло (т.е. не по X)
            Instantiate(bulletPrefab, firePoint.position, rotation);
        }
    }
}
