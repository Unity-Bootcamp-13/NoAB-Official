using System.Collections.Generic;
using UnityEngine;

public class CassidyUlt : MonoBehaviour
{
    [SerializeField] private ZomnicPoolManager zomnicPoolManager;
    [SerializeField] private GameObject UltMarkerPrefab;
    [SerializeField] private Transform UltMarkerParent;
    [SerializeField] private Rigidbody TumbleweedRB;
    [SerializeField] AudioSource UltStartSound;
    [SerializeField] AudioSource UltAttackSound;

    private List<(Zomnic zomnic, float distance)> _sortedTargets = new List<(Zomnic zomnic, float distance)>();
    private Dictionary<Zomnic, float> _damageTimers = new Dictionary<Zomnic, float>();
    private HashSet<Zomnic> _targetSet = new HashSet<Zomnic>();
    private Queue<Zomnic> _shotOrder = new Queue<Zomnic>();
    private Dictionary<Zomnic, UltMarkerUI> _ultMarkers = new Dictionary<Zomnic, UltMarkerUI>();

    internal static float currentUltPoint;
    private UltimateSettings _deadeye;
    private bool _isUltActive = false;
    private float _tumbleweedTimer = 0;

    public bool IsUltActive { get { return _isUltActive; } }
    public bool isUltimatePossible { get { return currentUltPoint >= _deadeye.maxUltimatePoint; } }

    private void Awake()
    {
        currentUltPoint = 1500;
        TumbleweedRB.gameObject.SetActive(false);
    }

    private void OnEnable()
    {        
        Zomnic.OnZomnicDamaged += IncreaseUltPointByDamage;
    }

    private void OnDisable()
    {
        Zomnic.OnZomnicDamaged -= IncreaseUltPointByDamage;
    }

    private void Update()
    {
        if (!_isUltActive)
            IncreaseUltPointPerSecond();

        _tumbleweedTimer += Time.deltaTime;

        if (_tumbleweedTimer > 10)
        {
            TumbleweedRB.gameObject.SetActive(false);
        }

        if (!_isUltActive)
            return;

        TrackInput();
        IncreaseDamage();
    }

    public void OnFirstUltimateButtonInput()
    {
        if (!_deadeye.isUltimatePossible)
        {
            Debug.Log("궁극기 사용 불가");
            return;
        }

        UltStartSound.Play();

        currentUltPoint = 0;
        _isUltActive = true;

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
            CreateUltMarker(entry.zomnic);
        }

        RollTumbleweed();
    }

    public void OnSecondUltimateButtonInput()
    {
        if (!_isUltActive) return;

        UltStartSound.Stop();

        FireUltimate();

        foreach (var zomnic in _ultMarkers.Keys)
        {
            Destroy(_ultMarkers[zomnic].gameObject);
        }
        _ultMarkers.Clear();

        _isUltActive = false;
        _deadeye.isUltimatePossible = false;
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

            CreateUltMarker(zomnic);
        }
    }

    public void IncreaseDamage()
    {
        if (!_deadeye.isUltimatePossible) return;

        foreach (Zomnic zomnic in _targetSet)
        {
            if (!IsObjectVisibleToCamera(zomnic.gameObject)) continue;

            float timer = _damageTimers[zomnic] + Time.deltaTime;
            _damageTimers[zomnic] = timer;

            int damage = (int)(_damageTimers[zomnic] * _deadeye.damagePerSecond);

            UpdateUIMarker(zomnic, damage);
        }
    }

    public void FireUltimate()
    {
        if (!_deadeye.isUltimatePossible) return;

        while (_shotOrder.Count > 0)
        {
            Zomnic zomnic = _shotOrder.Dequeue();
            if (!IsObjectVisibleToCamera(zomnic.gameObject)) continue;

            int damage = (int)(_damageTimers[zomnic] * _deadeye.damagePerSecond);
            
            zomnic.TakeDamage(damage);
            UltAttackSound.Play();
        }

        currentUltPoint = 0;
    }

    public void IncreaseUltPointPerSecond()
    {
        if (currentUltPoint < _deadeye.maxUltimatePoint)
        {
            currentUltPoint += Time.deltaTime * _deadeye.pointPerSecond;
            currentUltPoint = Mathf.Min(currentUltPoint, _deadeye.maxUltimatePoint);
        }
        else
        {
           // Debug.Log("궁극기사용가능");
            _deadeye.isUltimatePossible = true;
        }
    }

    public void IncreaseUltPointByDamage(float damage)
    {
        float point = damage * _deadeye.pointPerDamage;
        currentUltPoint = Mathf.Min(currentUltPoint + point, _deadeye.maxUltimatePoint);

        if (currentUltPoint >= _deadeye.maxUltimatePoint)
            _deadeye.isUltimatePossible = true;
    }

    public bool IsObjectVisibleToCamera(GameObject go)
    {
        if (!go.gameObject.activeSelf) return false;

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

    private void CreateUltMarker(Zomnic zomnic)
    {
        if (_ultMarkers.ContainsKey(zomnic)) return;
        if (!IsObjectVisibleToCamera(zomnic.gameObject)) return;

        GameObject markerGO = Instantiate(UltMarkerPrefab, UltMarkerParent);
        UltMarkerUI markerUI = markerGO.GetComponent<UltMarkerUI>();

        Transform headTransform = zomnic.GetHeadTransform(); // 머리 위치 Transform (직접 만들거나 바꿔도 됨)
        markerUI.Init(zomnic, headTransform);

        _ultMarkers.Add(zomnic, markerUI);
    }

    private void UpdateUIMarker(Zomnic zomnic, int damage)
    {
        if (!_ultMarkers.ContainsKey(zomnic)) return;

        if (damage >= zomnic.CurrentHP &&
            IsObjectVisibleToCamera(zomnic.gameObject))
        {
            _ultMarkers[zomnic].scale = 1f;
            _ultMarkers[zomnic].skullIcon.gameObject.SetActive(true);
        }
        else if (damage < zomnic.CurrentHP &&
                 IsObjectVisibleToCamera(zomnic.gameObject))
        {
            _ultMarkers[zomnic].scale = 1f;
            float outerCircleScale = -(1 / (float)zomnic.CurrentHP) * damage + 1.65f;
            _ultMarkers[zomnic].outerCircle.transform.localScale = Vector3.one * outerCircleScale;
        }
        else
        {
            _ultMarkers[zomnic].scale = 0f;
        }

        _ultMarkers[zomnic].transform.localScale = Vector3.one * _ultMarkers[zomnic].scale;
    }

    private void RollTumbleweed()
    {
        TumbleweedRB.gameObject.SetActive(true);
        TumbleweedRB.gameObject.transform.position = transform.position + (transform.forward * 4 - transform.right * 4);
        _tumbleweedTimer = 0;

        Vector3 right = transform.right;

        TumbleweedRB.AddForce(right * 5, ForceMode.Impulse);
    }       
}