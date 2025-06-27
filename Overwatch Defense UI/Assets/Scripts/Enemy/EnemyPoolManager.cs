using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct enemyPrefabEntries
{
    public EnemyType type;
    public GameObject prefab;
}

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private List<enemyPrefabEntries> enemyPrefabEntries;
    [SerializeField] private int initialZomnicPoolSize = 20;

    private Dictionary<EnemyType, GameObject> enemyPrefabs;
    private Dictionary<EnemyType, Queue<GameObject>> enemyPools = new();


    private void Awake()
    {
        enemyPrefabs = enemyPrefabEntries.ToDictionary(e => e.type, e => e.prefab);
        InitialInstantiate();
    }

    public void InitialInstantiate()
    {
        foreach (KeyValuePair<EnemyType, GameObject> kvp in enemyPrefabs)
        {
            if (kvp.Key == EnemyType.Zomnic)
            {
                Queue<GameObject> queue = new Queue<GameObject>();

                for (int i = 0; i < initialZomnicPoolSize; i++)
                {
                    GameObject zomnic = Instantiate(kvp.Value, transform);

                    zomnic.SetActive(false);
                    queue.Enqueue(zomnic);
                }

                enemyPools[kvp.Key] = queue;
            }
        }
    }

    public GameObject GetEnemy(EnemyType enemyType)
    {
        if (enemyPools[enemyType].Count > 0)
        {
            return Activate(enemyPools[enemyType].Dequeue());
        }

        enemyPrefabEntries prefab = enemyPrefabEntries.Find(e => e.type == enemyType);
        return Activate(Instantiate(prefab.prefab, transform));
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        Enemy e = enemy.GetComponent<Enemy>();
        e.ResetStatus();
        enemyPools[e.enemyType].Enqueue(enemy);
    }

    private GameObject Activate(GameObject obj)
    {
        obj.SetActive(true);
        return obj;
    }
}