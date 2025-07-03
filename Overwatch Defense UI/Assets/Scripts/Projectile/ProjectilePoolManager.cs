using System;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;


[Serializable]
public struct PoolConfig
{
    public int id;
    public GameObject prefab;
}


public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private List<PoolConfig> poolConfigs;

    private Dictionary<GameObject, IObjectPool<Projectile>> _poolsByPrefab;
    private Dictionary<int, IObjectPool<Projectile>> _poolsById;

    private void Awake()
    {
        _poolsById = new Dictionary<int, IObjectPool<Projectile>>(poolConfigs.Count);

        foreach (PoolConfig poolConfig in poolConfigs)
        {
            IObjectPool<Projectile> pool = CreatePool(poolConfig.prefab);
            _poolsById[poolConfig.id] = pool;
        }
    }

    private IObjectPool<Projectile> CreatePool(GameObject prefab)
    {
        return new ObjectPool<Projectile>(
            createFunc: () =>
            {
                GameObject go = Instantiate(prefab, transform);
                Projectile prj = go.GetComponent<Projectile>();
                prj.InjectPoolManager(this);
                return prj;
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false)
        );
    }

    public Projectile GetProjectile(int id)
    {
        _poolsById.TryGetValue(id, out IObjectPool<Projectile> pool);

        return pool.Get();
    }

    public void ReturnProjectile(Projectile prj) => _poolsById[prj.Id].Release(prj);
}
