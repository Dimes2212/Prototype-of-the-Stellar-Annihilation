using UnityEngine;

public class DoorHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Метод для получения урона
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Door took {amount} damage. Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод уничтожения двери
    private void Die()
    {
        Debug.Log("Door destroyed!");
        // Здесь можно проиграть анимацию, вызвать эффект, отключить коллайдер и т.п.
        Destroy(gameObject);
    }
}
