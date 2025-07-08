using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _volumeText;
    [SerializeField] private Slider _volumeSlider;


    void Update()
    {
        int volume = (int) (_volumeSlider.value * 100);
        _volumeText.text = volume.ToString();
    }
}
