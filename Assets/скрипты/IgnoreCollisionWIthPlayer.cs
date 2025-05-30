using UnityEngine;

public class IgnoreCollisionWithPlayer : MonoBehaviour
{
    private void Start()
    {
        // Получаем все коллайдеры на этом объекте (оружии)
        Collider[] weaponColliders = GetComponentsInChildren<Collider>();

        // Находим все объекты с тегом "Player"
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playerObjects)
        {
            Collider[] playerColliders = player.GetComponentsInChildren<Collider>();

            // Игнорируем столкновения между каждым коллайдером оружия и тела игрока
            foreach (Collider weaponCol in weaponColliders)
            {
                foreach (Collider playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(weaponCol, playerCol, true);
                }
            }
        }
    }
}
