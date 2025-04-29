using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public float speed = 100f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
