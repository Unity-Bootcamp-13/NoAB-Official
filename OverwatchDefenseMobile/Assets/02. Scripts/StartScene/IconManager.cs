using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [SerializeField] private Image _profilePortrait;
    [SerializeField] private Image _mainPlayerPortrait;
    [SerializeField] private Image _mainProfilePortrait;

    public void ImageChange(Image clickedPortrait)
    {
        Sprite newSprite = clickedPortrait.sprite;

        _profilePortrait.sprite = newSprite;
        _mainPlayerPortrait.sprite = newSprite;
        _mainProfilePortrait.sprite = newSprite;
    }
}


