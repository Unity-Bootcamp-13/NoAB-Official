using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemyPoolManager enemyPoolManager;
    [SerializeField] WaveManager waveManager;

    private void Awake()
    {
        waveManager.Inject(enemyPoolManager);
    }
}