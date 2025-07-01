using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoint;
    [SerializeField] private ZomnicPoolManager zomnicPoolManager;
    [SerializeField] private int spawnZomnicCount = 5;
    [SerializeField] private float spawnInterval = 1f;

    [Tooltip("Time from the last monster spawn to the start of the next wave")]
    [SerializeField] private float waveTimer = 15f;

    private int _waveCount = 1;
    private WaitForSeconds waitForNextWave;
    private WaitForSeconds waitForNextMonster;

    private void Awake()
    {
        waitForNextWave = new WaitForSeconds(waveTimer);
        waitForNextMonster = new WaitForSeconds(spawnInterval);
    }

    private void Start()
    {
        StartCoroutine(WaveCo());
    }

    IEnumerator WaveCo()
    {
        while (true)
        {
            yield return StartCoroutine(ZomnicSpawnCo(RandomSpawnPoint(), spawnZomnicCount));

            yield return waitForNextWave;
            _waveCount++;
        }
    }

    IEnumerator ZomnicSpawnCo(Vector3 spawnPoint, int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            yield return waitForNextMonster;

            GameObject zomnic = zomnicPoolManager.GetZomnic(spawnPoint);
        }
    }

    private Vector3 RandomSpawnPoint()
    {
        int idx = Random.Range(0, spawnPoint.Length - 1);
        return spawnPoint[idx].transform.position;
    }
}