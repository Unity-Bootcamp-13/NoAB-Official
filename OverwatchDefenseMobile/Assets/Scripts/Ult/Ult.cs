using System.Collections.Generic;
using UnityEngine;

public class Ult : MonoBehaviour
{
    [SerializeField] private ZomnicPoolManager zomnicPoolManager;

    private Dictionary<float, GameObject> _enemyDictionary = new Dictionary<float, GameObject>();    
    private Dictionary<Zomnic, float> _damageDictionary = new Dictionary<Zomnic, float>();
    private List<Zomnic> _targetList = new List<Zomnic>();
    private List<float> _distanceList = new List<float>();
    private Queue<GameObject> _shotOrder = new Queue<GameObject>();

    private float _initUltPoint;
    internal float _currentUltPoint = 0;
    private Dictionary<GameObject, int> _zomnicPreviousHpDictionary = new Dictionary<GameObject, int>();



    public void FirstInput(float initUiGauge)
    {      

        Debug.Log("처음 누름");
        _initUltPoint = initUiGauge;

        _enemyDictionary.Clear();
        _distanceList.Clear();
        _targetList.Clear();
        _damageDictionary.Clear();

        foreach (GameObject zomnic in zomnicPoolManager.zomnicList)
        {
            if (!zomnic.activeSelf) continue;
            if (!IsObjectVisibleToCamera(zomnic)) continue;

            float distance = Vector3.Distance(Camera.main.transform.position, zomnic.transform.position);

            _enemyDictionary.Add(distance, zomnic);
            _distanceList.Add(distance);
        }

        _distanceList.Sort();
        for (int i = 0; i < _distanceList.Count; i++)
        {
            _shotOrder.Enqueue(_enemyDictionary[_distanceList[i]]);

            _targetList.Add(_enemyDictionary[_distanceList[i]].gameObject.GetComponent<Zomnic>());
        }
    }

    public void TrackInput()
    {
        // if (!_canUseUltimate) return;

        Debug.Log("계속 누르고 있음");
        foreach (GameObject zomnic in zomnicPoolManager.zomnicList)
        {
            if (!zomnic.activeSelf) continue;
            if (_targetList.Contains(zomnic.gameObject.GetComponent<Zomnic>())) continue;
            if (!IsObjectVisibleToCamera(zomnic)) continue;

            _shotOrder.Enqueue(zomnic);
            _targetList.Add(zomnic.gameObject.GetComponent<Zomnic>());
        }
    }

    public void IncreaseDamage()
    {
        // if (!_canUseUltimate) return;

        Debug.Log("데미지 증가중");
        foreach (Zomnic zomnic in _targetList)
        {
            if (!IsObjectVisibleToCamera(zomnic)) continue;
            if (!_damageDictionary.ContainsKey(zomnic))
            {
                _damageDictionary.Add(zomnic, 0);
                continue;
            }
            if (_damageDictionary[zomnic] >= 200) continue;

            float timer = 0;

            timer += Time.deltaTime;
            _damageDictionary[zomnic] += timer * 150;
            if (timer > 2f)
                _damageDictionary[zomnic] += timer * 150;
        }
    }

    public void FireUltimate()
    {
        // if (!_canUseUltimate) return;

        Debug.Log("궁극기 발사");
        while (_shotOrder.Count > 0)
        {
            Zomnic zomnic = _shotOrder.Dequeue().GetComponent<Zomnic>();

            if (!IsObjectVisibleToCamera(zomnic)) continue;

            zomnic.TakeDamage((int)_damageDictionary[zomnic]);
        }

        EndUltEffect();
    }

    public void IncreaseUltgaugeOverTime()
    {
        if (_currentUltPoint < _initUltPoint)
        {
            _currentUltPoint += Time.deltaTime * 500;
        }
        else
        {
            Debug.Log("궁극기사용가능");
            //_canUseUltimate = true;
        }

        if (_currentUltPoint > 1800)
        {
            _currentUltPoint = 1800;
        }
    }

    public void IncreaseUltgaugeByDamage()
    {
        foreach (GameObject zomnic in zomnicPoolManager.zomnicList)
        {
            if (!zomnic.activeSelf)
            {
                _zomnicPreviousHpDictionary.Remove(zomnic);
                continue;
            }

            int currentHp = zomnic.GetComponent<Zomnic>().CurrentHp;

            if (currentHp >= 200) continue;

            if (!_zomnicPreviousHpDictionary.ContainsKey(zomnic))
                _zomnicPreviousHpDictionary[zomnic] = 200;

            int previousHp = _zomnicPreviousHpDictionary[zomnic];
            if (previousHp > currentHp)
            {
                int damageAmount = previousHp - currentHp;
                _currentUltPoint += damageAmount;
                Debug.Log($"궁극기 게이지 증가: {damageAmount}");
                _zomnicPreviousHpDictionary[zomnic] = currentHp;
            }

            if (_currentUltPoint > 1800)
            {
                _currentUltPoint = 1800;
            }
        }
    }

    private void StartUltEffect()
    {
        // 이속감소
    }

    private void EndUltEffect()
    {
        _currentUltPoint = 0;
        // 재장전 - 시작 혹은 끝에 
        // 이속회복
    }

    public bool IsObjectVisibleToCamera(GameObject gameObject)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        bool isInCameraView = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                             viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                             viewportPoint.z > 0;

        if (!isInCameraView)
            return false;

        Vector3 directionToTarget = (gameObject.transform.position - Camera.main.transform.position).normalized;
        float distanceToTarget = Vector3.Distance(Camera.main.transform.position, gameObject.transform.position);

        int layerMask = ~LayerMask.GetMask("Zomnic", "Transparency");

        bool isVisible = !Physics.Raycast(Camera.main.transform.position, directionToTarget, distanceToTarget - 0.1f, layerMask);

        return isVisible;
    }

    public bool IsObjectVisibleToCamera(Zomnic zomnic)
    {
        GameObject gameObject = zomnic.gameObject;

        return IsObjectVisibleToCamera(gameObject);
    }
}