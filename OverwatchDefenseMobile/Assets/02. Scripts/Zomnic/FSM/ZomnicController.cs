using UnityEngine;

public class ZomnicController : MonoBehaviour
{
    [SerializeField] private Zomnic zomnic;

    [Header("Base Arrival Detection Range")]
    [SerializeField] private float baseRange = 3.0f;

    [Header("SelfDesturct switch waiting time after arrive to the Base")]
    [SerializeField] private float selfDestructDelay = 2.0f;

    [Header("Dead switch waiting time after SelfDestruct")]
    [SerializeField] private float DeadDelay = 4.0f;

    internal bool isSelfDestructDead = false;
    private bool _reachedBasePoint = false;
    private float _reachTimer = 0f;
    private bool _hasTriggeredSelfDestruct = false;
    private float _selfDestructTimer = 0f;

    private void OnEnable()
    {
        _reachedBasePoint = false;
        _reachTimer = 0f;
        _hasTriggeredSelfDestruct = false;
        isSelfDestructDead = false;
        _selfDestructTimer = 0f;
    }

    private void Update()
    {
        if (zomnic.IsDead)
        {
            zomnic.Animator.SetTrigger("dead");
            return;
        }

        float distance = Vector3.Distance(transform.position, zomnic.BasePoint);
        zomnic.Animator.SetBool("isMoving", distance > baseRange);

        if (distance <= baseRange)
        {
            _reachedBasePoint = true;
            zomnic.Animator.SetBool("isMoving", false);
        }

        if (_hasTriggeredSelfDestruct && (_selfDestructTimer += Time.deltaTime) >= DeadDelay)
        {
            zomnic.Animator.SetTrigger("dead");
            isSelfDestructDead = true;
        }

        if (_reachedBasePoint && (_reachTimer += Time.deltaTime) >= selfDestructDelay)
        {
            zomnic.Animator.SetBool("isMoving", false);
            zomnic.Animator.SetBool("isSelfDestructing", true);
            _hasTriggeredSelfDestruct = true;
        }
    }
}