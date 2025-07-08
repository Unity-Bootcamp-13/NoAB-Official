using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private AsyncOperation sceneLoader;

    private void Start()
    {
        StartCoroutine(C_LoadInGameScene());
    }


    private void Update()
    {
        if (sceneLoader.isDone) sceneLoader.allowSceneActivation = true;
    }


    private IEnumerator C_LoadInGameScene()
    {
        sceneLoader = SceneManager.LoadSceneAsync("InGameScene");
        sceneLoader.allowSceneActivation = false;

        while (sceneLoader.progress < 0.9f)
        {
            yield return new WaitForSeconds(5f);
        }

        sceneLoader.allowSceneActivation = true;
    }
}
