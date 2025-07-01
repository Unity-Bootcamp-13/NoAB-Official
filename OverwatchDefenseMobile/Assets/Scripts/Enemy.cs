using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public TextMesh text;
    public int hp;

    private void Start()
    {
        hp = 1000;
    }


    private void Update()
    {
        text.text = hp.ToString();
    }


    public void Damage(int atk)
    {
        hp -= atk;
    }
}
