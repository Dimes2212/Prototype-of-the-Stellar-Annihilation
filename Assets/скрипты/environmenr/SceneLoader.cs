using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string loadingSceneName = "LoadingScene";
    public string targetSceneName = "GameScene";
    public float minLoadingDisplayTime = 1.5f; // Минимальное время показа экрана загрузки

    public void LoadSceneWithBuffer()
    {
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        // 1. Загружаем сцену загрузки ADDITIVE (добавляем поверх текущей)
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);

        // 2. Ждём минимум 1 кадр для инициализации UI загрузки
        yield return null;

        // 3. Начинаем загрузку основной сцены
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        asyncLoad.allowSceneActivation = false; // Важно: запрещаем авто-переход

        float timer = 0f;

        // 4. Ждём завершения загрузки И минимальное время показа
        while (!asyncLoad.isDone || timer < minLoadingDisplayTime)
        {
            timer += Time.deltaTime;

            // Когда загрузка почти завершена (90%) и прошло минимальное время
            if (asyncLoad.progress >= 0.9f && timer >= minLoadingDisplayTime)
            {
                asyncLoad.allowSceneActivation = true; // Разрешаем переход
            }

            // Здесь можно обновлять прогресс-бар, если есть
            // loadingProgressBar.value = asyncLoad.progress;

            yield return null;
        }

        // 5. Выгружаем сцену загрузки (опционально)
        yield return SceneManager.UnloadSceneAsync(loadingSceneName);
    }
}