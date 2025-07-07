using UnityEngine;
using UnityEngine.UI;

public class BaseHp 
{     
    [SerializeField] private Slider _slider;
    public static int currentHp = 3000;          
    public bool GameOver => currentHp <= 0;              

    public static void TakeDamage(int damage)
    {      
        if (currentHp < damage)
            damage = currentHp;

        currentHp -= damage;

        Debug.Log($"거점 체력 : {currentHp}");      
    }
}