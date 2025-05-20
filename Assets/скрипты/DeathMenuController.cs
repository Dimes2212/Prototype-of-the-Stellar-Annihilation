using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuController : MonoBehaviour
{
    public GameObject deathMenuUI;
    public Health playerHealth;

    public void ContinueGame()
    {
        if (playerHealth != null)
        {
            // Телепорт в зону возрождения
            Transform respawn = playerHealth.GetRespawnPoint();
            if (respawn != null)
            {
                GameObject player = playerHealth.gameObject;
                player.transform.position = respawn.position;
                player.transform.rotation = respawn.rotation;
            }

            // Восстановление HP
            playerHealth.RestoreHealth();
        }

        // Скрытие меню, если требуется
        if (deathMenuUI != null)
            deathMenuUI.SetActive(false);

        // Продолжение игры
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
