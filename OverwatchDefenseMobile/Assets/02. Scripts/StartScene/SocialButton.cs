using System.Collections;
using UnityEngine;

public class SocialButton : MonoBehaviour
{
    [SerializeField] private GameObject _selectEffect;
    [SerializeField] private GameObject _socialPanel;


    public void HeroPanel()
    {
        StartCoroutine(C_OpenSocialPanel());
    }

    private IEnumerator C_OpenSocialPanel()
    {
        _selectEffect.SetActive(true);
        _socialPanel.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _selectEffect.SetActive(false);
    }
}
