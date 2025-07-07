using UnityEngine;

public class BaseHealth 
{     
    public static int currentHp = 3000;        
  
    public bool GameOver => currentHp <= 0;          

    public static void TakeDamage(int damage)
    {      
        if (currentHp < damage)
            damage = currentHp;

        currentHp -= damage;

        Debug.Log($"���� ü�� : {currentHp}");      
    }
}