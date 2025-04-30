using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string loadingSceneName = "LoadingScene"; // Название буферной сцены
    public string targetSceneName = "GameScene";      // Название финальной сцены (куда переходить)

    public void LoadSceneWithBuffer()
    {
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        // Сначала загружаем буферную Loading сцену
        SceneManager.LoadScene(loadingSceneName);

        // Ждём один кадр
        yield return null;

        // Теперь грузим целевую сцену в фоне
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);

        // Ждём пока не загрузится полностью
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
