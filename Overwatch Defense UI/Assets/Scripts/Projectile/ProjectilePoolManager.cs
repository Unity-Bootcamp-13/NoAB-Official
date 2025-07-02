using UnityEngine;
using UnityEngine.Pool;


public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject peacekeeperPrefab;
    [SerializeField] private GameObject flashbangPrefab;
    private IObjectPool<Projectile> peacekeeperObjectPool;
    private IObjectPool<Projectile> flashbangObjectPool;

    private void Awake()
    {
        peacekeeperObjectPool = new ObjectPool<Projectile>(
            createFunc: () =>
            {
                GameObject go = Instantiate(peacekeeperPrefab, transform);
                Projectile prj = go.GetComponent<Projectile>();
                prj.InjectPoolManager(this);
                return prj;
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false)
            );

        flashbangObjectPool = new ObjectPool<Projectile>(
            createFunc: () =>
            {
                GameObject go = Instantiate(flashbangPrefab, transform);
                Projectile prj = go.GetComponent<Projectile>();
                prj.InjectPoolManager(this);
                return prj;
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false)
            );
    }

    public Projectile GetPeacekeeper() => peacekeeperObjectPool.Get();
    public Projectile GetFlashbang() => flashbangObjectPool.Get();
    public void ReturnPeacekeeper(Projectile projectile) => peacekeeperObjectPool.Release(projectile);
    public void ReturnFlashbang(Projectile projectile) => flashbangObjectPool.Release(projectile);
}
