using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DamageDetector>(out DamageDetector detector))
        {
            detector.OnDamageDetected(damage);
        }
    }
}
