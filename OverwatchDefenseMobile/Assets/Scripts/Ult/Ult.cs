using System.Collections.Generic;
using UnityEngine;

public class Ult : MonoBehaviour
{
    [SerializeField] private ZomnicPoolManager zomnicPoolManager;

    private Dictionary<float, GameObject> _enemyDictionary = new Dictionary<float, GameObject>();
    private List<Zomnic> _targetList = new List<Zomnic>();
    private Dictionary<Zomnic, float> _damageDictionary = new Dictionary<Zomnic, float>();
    private List<float> _distanceList = new List<float>();
    private Queue<GameObject> _shotOrder = new Queue<GameObject>();
    

    public void FirstInput()
    {
        Debug.Log("처음 누름");
        _enemyDictionary.Clear();
        _distanceList.Clear();
        _targetList.Clear();
        _damageDictionary.Clear();

        foreach (GameObject enemy in zomnicPoolManager.zomnicList)
        {
            if (!enemy.activeSelf) continue;
            if (!IsObjectVisibleToCamera(enemy)) continue;

            float distance = Vector3.Distance(Camera.main.transform.position, enemy.transform.position);

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

    public void TrackInput()
    {
        Debug.Log("계속 누르고 있음");
        foreach (GameObject enemy in zomnicPoolManager.zomnicList)
        {
            if (!enemy.activeSelf) continue;
            if (_targetList.Contains(enemy.gameObject.GetComponent<Zomnic>())) continue;
            if (!IsObjectVisibleToCamera(enemy)) continue;

            _shotOrder.Enqueue(enemy);
            _targetList.Add(enemy.gameObject.GetComponent<Zomnic>());
        }
    }

    public void IncreaseDamage()
    {
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
        Debug.Log("궁극기 발사");
        while (_shotOrder.Count > 0)
        {
            Zomnic zomnic = _shotOrder.Dequeue().GetComponent<Zomnic>();

            if (!IsObjectVisibleToCamera(zomnic)) continue;

            zomnic.TakeDamage((int)_damageDictionary[zomnic]);
        }
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

        bool isVisible = !Physics.Raycast(Camera.main.transform.position, directionToTarget, distanceToTarget - 0.1f);

        return isVisible;
    }

    public bool IsObjectVisibleToCamera(Zomnic zomnic)
    {
        GameObject gameObject = zomnic.gameObject;

        return IsObjectVisibleToCamera(gameObject);
    }
}
