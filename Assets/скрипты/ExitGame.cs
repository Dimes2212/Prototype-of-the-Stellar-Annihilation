using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        // В редакторе Unity эта команда не сработает, только в билде
        Application.Quit();

        // Чтобы видеть результат в редакторе:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
