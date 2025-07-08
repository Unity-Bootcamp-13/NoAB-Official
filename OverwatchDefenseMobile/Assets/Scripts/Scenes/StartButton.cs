using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject _selectEffect;

    public void LoadInGameScene()
    {
        StartCoroutine(C_Loading());
    }

    private IEnumerator C_Loading()
    {
        _selectEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync("LoadingScene");
        yield return null;
    }
}
