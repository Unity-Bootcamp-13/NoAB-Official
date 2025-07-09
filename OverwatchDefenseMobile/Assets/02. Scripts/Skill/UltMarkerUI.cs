using UnityEngine;
using UnityEngine.UI;

public class UltMarkerUI : MonoBehaviour
{
    private Zomnic _zomnic;
    private Transform _targetTransform;
    public float scale;
    public GameObject ultMarker;
    public Image skullIcon;
    public Image outerCircle;

    public void Init(Zomnic zomnic, Transform headTransform)
    {
        _targetTransform = headTransform;
        _zomnic = zomnic;
        scale = 0;
    }


    private void Update()
    {
        if (!_zomnic.gameObject.activeSelf)
        {
            ultMarker.transform.localScale = Vector3.zero;
            return;
        }

        Vector3 worldPos = _targetTransform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0f)
        {
            ultMarker.transform.localScale = Vector3.zero;
            return;
        }

        transform.position = screenPos;
    }
}