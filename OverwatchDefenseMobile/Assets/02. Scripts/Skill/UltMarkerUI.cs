using UnityEngine;
using UnityEngine.UI;

public class UltMarkerUI : MonoBehaviour
{
    public Image skullImage;

    private Zomnic _zomnic;
    private Transform targetTransform;
    public float _scale;

    public void Init(Zomnic zomnic, Transform headTransform)
    {
        targetTransform = headTransform;
        _zomnic = zomnic;
        _scale = 0;
    }


    private void Update()
    {
        if (!_zomnic.gameObject.activeSelf)
        {
            skullImage.rectTransform.localScale = Vector3.zero;
            return;
        }

        Vector3 worldPos = targetTransform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0f)
        {
            skullImage.rectTransform.localScale = Vector3.zero;
            return;
        }

        transform.position = screenPos;
    }
}