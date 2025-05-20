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
            
            Transform respawn = playerHealth.GetRespawnPoint();
            if (respawn != null)
            {
                GameObject player = playerHealth.gameObject;
                player.transform.position = respawn.position;
                player.transform.rotation = respawn.rotation;
            }

            
            playerHealth.RestoreHealth();
        }

        
        if (deathMenuUI != null)
            deathMenuUI.SetActive(false);

        
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
