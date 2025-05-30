using UnityEngine;
using System.Collections;

public class PlayerDamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 10f;
    public string targetTag = "Enemy";
    public float damageCooldown = 0.1f;

    [Header("Effects")]
    [SerializeField] private AudioSource weaponSound;
    [SerializeField] private GameObject bloodEffectPrefab;
    [SerializeField] private float effectDuration = 0.5f;
    [SerializeField] private Vector3 minRotation = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 maxRotation = new Vector3(0, 360, 0);

    private bool canDealDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        // Получаем точку контакта между оружием и врагом
        Vector3 contactPoint = other.ClosestPoint(transform.position);
        Vector3 contactNormal = (transform.position - contactPoint).normalized;

        // Проигрываем эффект крови в точке контакта
        SpawnBloodEffect(contactPoint, contactNormal, other.transform);

        // Наносим урон, если не в кулдауне
        if (canDealDamage)
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                weaponSound.Play();
                health.TakeDamage(damageAmount);
                StartCoroutine(DamageCooldown());
            }
            else
            {
               
            
            }
        }
    }

    private void SpawnBloodEffect(Vector3 position, Vector3 normal, Transform parent)
    {

        if (bloodEffectPrefab == null)
        {
            Debug.LogWarning("Blood effect prefab is not assigned!");
            return;
        }

        // Генерируем случайное вращение в заданных пределах
        Vector3 randomRotation = new Vector3(
            Random.Range(minRotation.x, maxRotation.x),
            Random.Range(minRotation.y, maxRotation.y),
            Random.Range(minRotation.z, maxRotation.z)
        );

        // Создаем эффект с учетом нормали поверхности и случайного вращения
        GameObject effect = Instantiate(
            bloodEffectPrefab,
            position,
            Quaternion.LookRotation(normal) * Quaternion.Euler(randomRotation),
            parent
        );

        // Удаляем эффект через заданное время
        Destroy(effect, effectDuration);
    }

    private IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDealDamage = true;
    }
}