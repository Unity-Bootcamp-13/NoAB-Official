using UnityEngine;
using UnityEngine.UI;

public class BaseHpBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private void Awake()
    {
        _slider.maxValue = BaseHp.currentHp;
        _slider.value = BaseHp.currentHp;
    }

    private void Update()
    {
        _slider.value = BaseHp.currentHp;
    }

}
