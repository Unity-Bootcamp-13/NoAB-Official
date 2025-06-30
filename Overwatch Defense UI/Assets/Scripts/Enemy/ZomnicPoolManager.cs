using UnityEngine;
using UnityEngine.Pool;

public class ZomnicPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject zomnicPrefab;
    [SerializeField] private int initialSize = 20;

    IObjectPool<GameObject> zomnicPool;


    void Awake()
    {
        zomnicPool = new ObjectPool<GameObject>(
            createFunc: () => {
                GameObject go = Instantiate(zomnicPrefab, transform);
                go.SetActive(false);
                ZomnicController ctrl = go.GetComponent<ZomnicController>();
                ctrl.InjectPoolManager(this);
                return go;
            },
            actionOnRelease: obj => obj.SetActive(false),
            collectionCheck: true,
            defaultCapacity: initialSize
        );

        for (int i = 0; i < initialSize; i++)
        {
            GameObject go = Instantiate(zomnicPrefab, transform);
            ZomnicController ctrl = go.GetComponent<ZomnicController>();
            ctrl.InjectPoolManager(this);

            zomnicPool.Release(go);
        }
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