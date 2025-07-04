using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Ult : MonoBehaviour
{
    [SerializeField] private ZomnicPoolManager _poolManager;

    private Dictionary<float, GameObject> _enemyDictionary = new Dictionary<float, GameObject>();
    private List<Zomnic> _targetList = new List<Zomnic>();
    private Dictionary<Zomnic, float> _damageDictionary = new Dictionary<Zomnic, float>();
    private List<float> _distanceList = new List<float>();
    private Queue<GameObject> _shotOrder = new Queue<GameObject>();
    private Camera _camera;

    private bool _buttonDown;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void OnPointerDown()
    {
        _buttonDown = true;
        FirstInput();
    }

    public void OnPointerUp()
    {
        _buttonDown = false;
        FireUltimate();
    }

    void Update()
    {
        if (_buttonDown)
        {
            TrackInput();
            IncreaseDamage();
        }


        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            FirstInput();
        }
        
        if (Keyboard.current.qKey.isPressed)
        {
            TrackInput();
            IncreaseDamage();
        }

        if (Keyboard.current.qKey.wasReleasedThisFrame)
        {
            FireUltimate();
        }
    }

    private void FirstInput()
    {
        Debug.Log("처음 누름");
        _enemyDictionary.Clear();
        _distanceList.Clear();
        _targetList.Clear();
        _damageDictionary.Clear();

        foreach (GameObject enemy in _poolManager.zomnicList)
        {
            if (!enemy.activeSelf) continue;
            if (!enemy.GetComponentInChildren<Renderer>().isVisible) continue;

            float distance = Vector3.Distance(_camera.transform.position, enemy.transform.position);

            _enemyDictionary.Add(distance, enemy);
            _distanceList.Add(distance);
        }

        _distanceList.Sort();
        for (int i = 0; i < _distanceList.Count; i++)
        {
            _shotOrder.Enqueue(_enemyDictionary[_distanceList[i]]);

            _targetList.Add(_enemyDictionary[_distanceList[i]].gameObject.GetComponent<Zomnic>());
        }
    }

    private void TrackInput()
    {
        Debug.Log("계속 누르고 있음");
        foreach (GameObject enemy in _poolManager.zomnicList)
        {
            if (!enemy.activeSelf) continue;
            if (_targetList.Contains(enemy.gameObject.GetComponent<Zomnic>())) continue;
            if (!enemy.GetComponentInChildren<Renderer>().isVisible) continue;

            _shotOrder.Enqueue(enemy);
            _targetList.Add(enemy.gameObject.GetComponent<Zomnic>());
        }
    }

    private void IncreaseDamage()
    {
        Debug.Log("데미지 증가중");
        foreach (Zomnic zomnic in _targetList)
        {
            if (!zomnic.GetComponentInChildren<Renderer>().isVisible) continue;
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
        Debug.Log("궁극기 발사");
        while (_shotOrder.Count > 0)
        {
            Zomnic zomnic = _shotOrder.Dequeue().GetComponent<Zomnic>();

            if (!zomnic.GetComponentInChildren<Renderer>().isVisible) continue;

            zomnic.TakeDamage((int)_damageDictionary[zomnic]);
        }
    }

    
}
