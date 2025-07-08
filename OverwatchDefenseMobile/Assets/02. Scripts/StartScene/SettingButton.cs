using System.Collections;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private GameObject _selectEffect;
    [SerializeField] private GameObject _settingWindow;


    public void SettingWindow()
    {
        StartCoroutine(C_OpenSettingWindow());
    }

    private IEnumerator C_OpenSettingWindow()
    {
        _selectEffect.SetActive(true);
        _settingWindow.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _selectEffect.SetActive(false);
    }
}
