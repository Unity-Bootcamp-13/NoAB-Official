using System;
using System.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public enum ZomnicState
{
    Idle,
    Move,
    SelfDestruct,
    Dead,
    Hit
}

public class ZomnicController : MonoBehaviour
{
    [SerializeField] private Zomnic zomnic;
    [SerializeField] private Vector3 basePoint;

    private ZomnicState _zomnicState = ZomnicState.Idle;
    private ZomnicPoolManager _zomnicPoolManager;
    private float _reachTimer = 0f;
    private bool _reachBasePoint = false;

    public void InjectPoolManager(ZomnicPoolManager poolManager)
    {
        _zomnicPoolManager = poolManager;
    }

    private void Update()
    {
        switch(_zomnicState)
        {
            case ZomnicState.Idle:
                ZomnicIdle();
                break;
            case ZomnicState.Move:
                ZomnicMove();
                break;
            case ZomnicState.SelfDestruct:
                ZomnicSelfDestruct();
                break;
            case ZomnicState.Dead:
                ZomnicDead();
                break;
            case ZomnicState.Hit:
                ZomnicHit();
                break;
        }

        if (zomnic.HP <= 0)
        {
            _zomnicState = ZomnicState.Dead;
        }

        float distance = Vector3.Distance(transform.position, basePoint);
        if (distance < 1)
        {
            _reachBasePoint = true;
            Debug.Log("µµÂøÇÔ");
        }

        if (_reachBasePoint == false)
            return;

        _reachTimer += Time.deltaTime;

        if (_zomnicState == ZomnicState.SelfDestruct)
            return;

        if (_reachTimer > 2f)
        {
            _zomnicState = ZomnicState.SelfDestruct;
            _reachTimer = 0;
        }
    }

    
    private void ZomnicIdle()
    {
        // TODO : Write Animation Code
        Debug.Log("Idle");
        if (gameObject.activeSelf == true)
            _zomnicState = ZomnicState.Move;
    }

    private void ZomnicMove()
    {
        // TODO : Write Animation Code
    }

    private void ZomnicSelfDestruct()
    {
        // TODO : Write Animation Code
        Debug.Log("SelfDestruct");
        _zomnicPoolManager.ReturnZomnic(gameObject);

    }

    private void ZomnicDead()
    {
        Debug.Log("Dead");
        _zomnicPoolManager.ReturnZomnic(gameObject);
    }
    private void ZomnicHit()
    {
        // TODO : Write Animation Code
    }
}