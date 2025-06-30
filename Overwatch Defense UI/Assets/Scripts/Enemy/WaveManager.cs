using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoint;
    [SerializeField] private ZomnicPoolManager zomnicPoolManager;

    [Tooltip("Time from the last monster spawn to the start of the next wave")]
    [SerializeField] private float waveTimer = 15f;

    private int _waveCount = 1;


    private void Start()
    {
        StartCoroutine(WaveCo());
    }

    IEnumerator WaveCo()
    {
        while (true)
        {
            int idx = Random.Range(0, spawnPoint.Length - 1);
            yield return StartCoroutine(FiveZomnicSpawnCo(spawnPoint[idx].transform.position));

            yield return new WaitForSeconds(waveTimer);
            _waveCount++;
        }
    }

    IEnumerator FiveZomnicSpawnCo(Vector3 spawnPoint)
    {
        int spawnCount = 0;
        float spawnInterval = 1f;

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(spawnInterval);

            GameObject zomnic = zomnicPoolManager.GetZomnic(spawnPoint);

            spawnCount++;
        }
    }
}