using System.Collections;
using UnityEngine;

public class HeroButton : MonoBehaviour
{
    [SerializeField] private GameObject _selectEffect;
    [SerializeField] private GameObject _heroPanel;


    public void HeroPanel()
    {
        StartCoroutine(C_OpenHeroPanel());
    }

    private IEnumerator C_OpenHeroPanel()
    {
        _selectEffect.SetActive(true);
        _heroPanel.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _selectEffect.SetActive(false);
    }
}
