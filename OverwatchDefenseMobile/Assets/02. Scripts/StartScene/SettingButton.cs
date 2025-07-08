using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private GameObject _selectEffect;

    public void SettingWindow()
    {
        _selectEffect.SetActive(true);
    }
}
