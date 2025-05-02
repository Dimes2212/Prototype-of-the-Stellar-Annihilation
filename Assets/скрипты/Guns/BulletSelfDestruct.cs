

using UnityEngine;

public class BulletSelfDestruct : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Удалить пулю через lifetime секунд
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon")) return;

        // Здесь можно добавить проверку, например:
        // if (other.CompareTag("Enemy")) { other.GetComponent<Health>().TakeDamage(damage); }

        Destroy(gameObject);
    }
}
