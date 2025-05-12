//using UnityEngine;

//public class AttackPoint : MonoBehaviour
//{
//    public bool IsOccupied { get; private set; }

//    public void SetOccupied(bool isOccupied)
//    {
//        IsOccupied = isOccupied;
//    }
//}

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttackPoint : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private bool isActive = true;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioClip hitSound;

    private BoxCollider boxCollider;

    public float DamageMultiplier => damageMultiplier;
    public bool IsActive => isActive;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public Vector3 GetRandomPoint()
    {
        if (boxCollider != null)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-boxCollider.size.x / 2, boxCollider.size.x / 2),
                Random.Range(-boxCollider.size.y / 2, boxCollider.size.y / 2),
                Random.Range(-boxCollider.size.z / 2, boxCollider.size.z / 2)
            );
            return transform.TransformPoint(boxCollider.center + randomPoint);
        }
        return transform.position;
    }

    public void PlayHitEffects()
    {
        if (hitEffect != null) hitEffect.Play();
        if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null) return;

        // Сохраняем текущую матрицу
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Устанавливаем матрицу трансформации коллайдера
        Gizmos.matrix = Matrix4x4.TRS(
            transform.TransformPoint(boxCollider.center),
            transform.rotation,
            transform.lossyScale
        );

        // Рисуем контур коллайдера
        Gizmos.color = new Color(1, 0, 0, 0.7f);
        Gizmos.DrawWireCube(Vector3.zero, boxCollider.size);

        // Восстанавливаем матрицу
        Gizmos.matrix = originalMatrix;
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null) return;

        // Сохраняем текущую матрицу
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Устанавливаем матрицу трансформации коллайдера
        Gizmos.matrix = Matrix4x4.TRS(
            transform.TransformPoint(boxCollider.center),
            transform.rotation,
            transform.lossyScale
        );

        // Рисуем полупрозрачный куб
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(Vector3.zero, boxCollider.size);

        // Восстанавливаем матрицу
        Gizmos.matrix = originalMatrix;
    }
}