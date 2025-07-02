using System;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed;
    public bool isHaveGravity;
    public Vector3 shootingAngle;

    void Start()
    {
        // angle 방향으로 addforce
    }


    public void OnColliderEnter()
    {
        // 닿은 후 처리
    }
}