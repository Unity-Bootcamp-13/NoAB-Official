using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject basePoint;
    [SerializeField] private GameObject[] spawnPoint;

    private EnemyPoolManager _enemyPoolManager;

    private float _waveTimer = 20f;
    private int _waveCount = 1;

    private void Update()
    {
        _waveTimer += Time.deltaTime;

        if (_waveTimer > 20f)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoint.Length - 1);

            StartCoroutine(FiveZomnicSpawn(spawnPoint[randomSpawnPoint].transform.position));

            _waveTimer = 0;
            _waveCount++;
        }
    }

    public void Inject(EnemyPoolManager enemyPoolManager)
    {
        _enemyPoolManager = enemyPoolManager;
    }

    IEnumerator FiveZomnicSpawn(Vector3 spawnPoint)
    {
        int spawnCount = 0;
        float spawnInterval = 1f;

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(spawnInterval);

            GameObject zomnic = _enemyPoolManager.GetEnemy(EnemyType.Zomnic);
            zomnic.transform.position = spawnPoint;
            zomnic.GetComponent<Zomnic>().Agent.Warp(spawnPoint);
            zomnic.GetComponent<Zomnic>().MoveToBasePoint(basePoint.transform.position);

            spawnCount++;
        }
    }
}