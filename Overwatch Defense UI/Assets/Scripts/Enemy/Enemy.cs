using UnityEngine;

public enum EnemyType
{
    None,
    Zomnic
}

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;

    public void ResetStatus()
    {
        // TO DO : HP Reset
    }
}