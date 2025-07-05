using System.Collections.Generic;
using UnityEngine;

public class CassidyUlt : MonoBehaviour
{
    [SerializeField] private ZomnicPoolManager zomnicPoolManager;

    private List<(Zomnic zomnic, float distance)> _sortedTargets = new List<(Zomnic zomnic, float distance)>();
    private Dictionary<Zomnic, float> _damageTimers = new Dictionary<Zomnic, float>();
    private HashSet<Zomnic> _targetSet = new HashSet<Zomnic>();
    private Queue<Zomnic> _shotOrder = new Queue<Zomnic>();

    internal float currentUltPoint;
    private UltimateSettings _deadeye;

    private void Awake()
    {
        currentUltPoint = 0;
    }

    private void Update()
    {
        IncreaseUltPointPerSecond();
    }

    public void FirstInput()
    {
        if (!_deadeye.isUltimatePossible)
        {
            Debug.Log("궁극기 사용 불가");
            return;
        }

        _deadeye.isUltimatePossible = false;

        _sortedTargets.Clear();
        _damageTimers.Clear();
        _targetSet.Clear();
        _shotOrder.Clear();

        // 수집 및 거리 계산
        foreach (Zomnic zomnic in zomnicPoolManager.zomnicList)
        {
            if (!zomnic.gameObject.activeSelf) continue;
            if (!IsObjectVisibleToCamera(zomnic.gameObject)) continue;

            Zomnic zom = zomnic.GetComponent<Zomnic>();
            float dist = Vector3.Distance(Camera.main.transform.position, zomnic.transform.position);
            _sortedTargets.Add((zom, dist));
        }

        // 거리순 정렬 및 큐에 추가
        _sortedTargets.Sort((a, b) => a.distance.CompareTo(b.distance));
        foreach ((Zomnic zomnic, float dist) entry in _sortedTargets)
        {
            _shotOrder.Enqueue(entry.zomnic);
            _targetSet.Add(entry.zomnic);
            _damageTimers[entry.zomnic] = 0f;
        }
    }

    public void TrackInput()
    {
        if (!_deadeye.isUltimatePossible) return;

        foreach (Zomnic zomnic in zomnicPoolManager.zomnicList)
        {
            if (!zomnic.gameObject.activeSelf ||
                _targetSet.Contains(zomnic)) continue;
            if (!IsObjectVisibleToCamera(zomnic.gameObject)) continue;

            _shotOrder.Enqueue(zomnic);
            _targetSet.Add(zomnic);
            _damageTimers[zomnic] = 0f;
        }
    }

    public void IncreaseDamage()
    {
        if (!_deadeye.isUltimatePossible) return;

        foreach (Zomnic zomnic in _targetSet)
        {
            if (!IsObjectVisibleToCamera(zomnic.gameObject)) continue;
            float timer = _damageTimers[zomnic] + Time.deltaTime;
            _damageTimers[zomnic] = Mathf.Min(timer, zomnic.MaxHP / _deadeye.damagePerSecond);
        }
    }

    public void FireUltimate()
    {
        if (!_deadeye.isUltimatePossible) return;

        while (_shotOrder.Count > 0)
        {
            Zomnic zomnic = _shotOrder.Dequeue();
            if (!IsObjectVisibleToCamera(zomnic.gameObject)) continue;

            _damageTimers.TryGetValue(zomnic, out float timer);
            int damage = Mathf.Min((int)(timer * _deadeye.damagePerSecond), zomnic.MaxHP);
            zomnic.TakeDamage(damage);
        }

        EndUltEffect();
    }

    public void IncreaseUltPointPerSecond()
    {
        if (currentUltPoint < _deadeye.maxUltimatePoint)
        {
            currentUltPoint += Time.deltaTime * _deadeye.ultimatePointPerSecond;
            currentUltPoint = Mathf.Min(currentUltPoint, _deadeye.maxUltimatePoint);
        }
        else
        {
            Debug.Log("궁극기사용가능");
            _deadeye.isUltimatePossible = true;
        }
    }

    public void IncreaseUltgaugeByDamage()
    {
        // 좀닉에서 받은 데미지를 게임매니저에 보고
        // --> 게임매니저에서 영웅에게 포인트 보고
        // 아님 이벤트로 구현..
    }

    private void StartUltEffect()
    {
        // 캐서디의 이속감소
    }

    private void EndUltEffect()
    {
        currentUltPoint = 0;
        // 재장전 - 시작 혹은 끝에 
        // 캐서디의 이속회복
    }

    public bool IsObjectVisibleToCamera(GameObject go)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(go.transform.position);
        bool isNotInCameraView = viewportPoint.z <= 0 ||
                                 viewportPoint.x < 0 ||
                                 viewportPoint.x > 1 ||
                                 viewportPoint.y < 0 ||
                                 viewportPoint.y > 1;

        if (isNotInCameraView) return false;

        Vector3 directionToTarget = (go.transform.position - Camera.main.transform.position).normalized;
        float distanceToTarget = Vector3.Distance(Camera.main.transform.position, go.transform.position);
        int layerMask = ~LayerMask.GetMask("Zomnic", "Transparency");
        bool isVisible = !Physics.Raycast(Camera.main.transform.position, directionToTarget, distanceToTarget - 0.1f, layerMask);

        return isVisible;
    }

    public void InjectUltimateSettings(UltimateSettings deadeye)
    {
        _deadeye = deadeye;
    }
}