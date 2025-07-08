using UnityEngine;
using UnityEngine.UI;

public class UltMarkerUI : MonoBehaviour
{
    public Image skullImage;

    private Zomnic _zomnic;
    private Transform targetTransform;
    private float _scale;

    public void Init(Zomnic zomnic, Transform headTransform)
    {
        targetTransform = headTransform;
        _zomnic = zomnic;
        _scale = 0;
    }

    public void UpdateDamage(float damage)
    {
        Debug.Log($"좀닉 현재 체력 {_zomnic.CurrentHP}");
        Debug.Log($"damage {damage}");

        if (damage >= _zomnic.CurrentHP)
            _scale = 1f;
        else
            _scale = 0f;        
      
        skullImage.rectTransform.localScale = Vector3.one * _scale;
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 worldPos = targetTransform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        transform.position = screenPos;
    }
}