using UnityEngine;
using UnityEngine.Pool;


public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject peacekeeperPrefab;
    [SerializeField] private GameObject flashbangPrefab;
    private IObjectPool<GameObject> peacekeeperObjectPool;
    private IObjectPool<GameObject> flashbangObjectPool;

    private void Awake()
    {
        peacekeeperObjectPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject go = Instantiate(peacekeeperPrefab, transform);
                return go;
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false)
            );

        flashbangObjectPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject go = Instantiate(flashbangPrefab, transform);
                return go;
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false)
            );
    }

    public GameObject GetPeacekeeper() => peacekeeperObjectPool.Get();
    public GameObject GetFlashbang() => flashbangObjectPool.Get();
    public void ReturnPeacekeeper(GameObject projectile) => peacekeeperObjectPool.Release(projectile);
    public void ReturnFlashbang(GameObject projectile) => flashbangObjectPool.Release(projectile);
}