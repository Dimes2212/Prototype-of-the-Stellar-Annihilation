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

    private BoxCollider _collider;

    private void Awake() => _collider = GetComponent<BoxCollider>();

    public Vector3 GetRandomPositionInZone()
    {
        // Генерируем случайную точку внутри коллайдера
        Vector3 randomPoint = new Vector3(
            Random.Range(-_collider.size.x / 2, _collider.size.x / 2),
            Random.Range(-_collider.size.y / 2, _collider.size.y / 2),
            Random.Range(-_collider.size.z / 2, _collider.size.z / 2)
        );

        // Трансформируем точку с учетом поворота и позиции
        return transform.TransformPoint(_collider.center + randomPoint);
    }

    public bool IsPositionInZone(Vector3 position)
    {
        // Конвертируем мировые координаты в локальные
        Vector3 localPos = transform.InverseTransformPoint(position) - _collider.center;

        // Проверяем находится ли точка внутри границ
        return Mathf.Abs(localPos.x) <= _collider.size.x / 2 &&
               Mathf.Abs(localPos.y) <= _collider.size.y / 2 &&
               Mathf.Abs(localPos.z) <= _collider.size.z / 2;
    }

    public void PlayHitEffects()
    {
        if (hitEffect != null) hitEffect.Play();
        if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }

    private void OnDrawGizmos()
    {
        if (_collider == null) _collider = GetComponent<BoxCollider>();

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        DrawColliderGizmo();
    }

    private void OnDrawGizmosSelected()
    {
        if (_collider == null) _collider = GetComponent<BoxCollider>();

        Gizmos.color = new Color(1, 0, 0, 0.7f);
        DrawColliderGizmo();

        // Пример случайной точки
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetRandomPositionInZone(), 0.2f);
    }

    private void DrawColliderGizmo()
    {
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(
            transform.TransformPoint(_collider.center),
            transform.rotation,
            transform.lossyScale
        );

        Gizmos.DrawWireCube(Vector3.zero, _collider.size);
        Gizmos.matrix = originalMatrix;
    }
}