using UnityEngine;
using UnityEngine.Pool;

public class ZomnicPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject zomnicPrefab;

    private IObjectPool<GameObject> zomnicPool;


    void Awake()
    {
        zomnicPool = new ObjectPool<GameObject>(
            createFunc: () => 
            {
                GameObject go = Instantiate(zomnicPrefab, transform);
                go.SetActive(false);
                Zomnic zomnic = go.GetComponent<Zomnic>();
                zomnic.InjectPoolManager(this);
                return go;
            },
            actionOnRelease: obj => obj.SetActive(false),
            collectionCheck: true
        );
    }

    public GameObject GetZomnic(Vector3 spawnPoint)
    {
        GameObject zomnic = zomnicPool.Get();
        zomnic.transform.position = spawnPoint;
        zomnic.SetActive(true);
        return zomnic;
    }
    public void ReturnZomnic(GameObject zomnicPrefab) => zomnicPool.Release(zomnicPrefab);
}