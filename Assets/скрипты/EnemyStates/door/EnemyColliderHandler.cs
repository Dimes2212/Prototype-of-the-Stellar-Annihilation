using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyColliderHandler : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true; // чтобы не конфликтовал с NavMeshAgent
        rb.constraints = RigidbodyConstraints.FreezeRotation; // не вращаем врага от физики
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Легкое отталкивание игрока — не даем застрять
            Vector3 pushDir = (collision.transform.position - transform.position).normalized;
            pushDir.y = 0;

            if (collision.rigidbody != null)
            {
                collision.rigidbody.AddForce(pushDir * 2f, ForceMode.Impulse);
            }
        }
    }
}
