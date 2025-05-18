using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string loadingSceneName = "LoadingScene";
    public string targetSceneName = "GameScene";
    public float minLoadingDisplayTime = 1.5f; 

    public void LoadSceneWithBuffer()
    {
        StartCoroutine(LoadSceneProcess());
    }
    private IEnumerator LoadSceneProcess()
    {
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
        yield return null;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        asyncLoad.allowSceneActivation = false;
        float timer = 0f;
        while (!asyncLoad.isDone || timer < minLoadingDisplayTime)
        {
            timer += Time.deltaTime;
            if (asyncLoad.progress >= 0.9f && timer >= minLoadingDisplayTime)
            {
                asyncLoad.allowSceneActivation = true; 
            }
            yield return null;
        }
        yield return SceneManager.UnloadSceneAsync(loadingSceneName);
    }
}