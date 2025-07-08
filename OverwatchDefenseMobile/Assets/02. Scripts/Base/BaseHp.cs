using UnityEngine;

public class BaseHp : MonoBehaviour
{
    public const int maxHp = 3000;
    internal static int currentHp;

    public static bool GameOver => currentHp <= 0;

    private void Awake()
    {
        currentHp = maxHp;
    }



    public static void TakeDamage(int damage)
    {      
        if (currentHp < damage)
            damage = currentHp;

        currentHp -= damage;

        Debug.Log($"거점 체력 : {currentHp}");      
    }
}