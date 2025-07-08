using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject _selectEffect;

    public void LoadInGameScene()
    {
        _selectEffect.SetActive(true);
        SceneManager.LoadSceneAsync("LoadingScene");
    }
}
