using UnityEngine;

public class DoorHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyDoor();  // Разрушаем дверь, когда здоровье становится 0
        }
    }

    private void DestroyDoor()
    {
        // Здесь можно добавить логику для уничтожения двери
        // Например, удаляем коллайдер или просто отключаем объект
        Debug.Log("Door destroyed!");
        Destroy(gameObject);  // Удаляем объект двери
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
