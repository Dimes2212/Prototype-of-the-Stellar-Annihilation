

//using UnityEngine;

//public class BulletSelfDestruct : MonoBehaviour
//{
//    public float speed = 20f;
//    public float lifetime = 5f;

//    void Start()
//    {
//        Destroy(gameObject, lifetime); // Удалить пулю через lifetime секунд
//    }

//    void Update()
//    {
//        transform.position += transform.forward * speed * Time.deltaTime;
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Weapon")) return;

//        // Здесь можно добавить проверку, например:
//        // if (other.CompareTag("Enemy")) { other.GetComponent<Health>().TakeDamage(damage); }

//        Destroy(gameObject);
//    }
//}


using UnityEngine;

public class BulletSelfDestruct : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;
    public LayerMask collisionMask; // Укажи, с какими слоями пуля должна сталкиваться

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollision(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        // Можно добавить проверку типа объекта
        if (hit.collider.CompareTag("Weapon")) return;

        // Например:
        // if (hit.collider.CompareTag("Enemy")) { hit.collider.GetComponent<Health>().TakeDamage(damage); }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon")) return;

        Destroy(gameObject);
    }
}
