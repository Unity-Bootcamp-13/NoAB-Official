using UnityEngine;

public class ZomnicController : MonoBehaviour
{
    [SerializeField] private Zomnic zomnic;

    [Header("Base Arrival Detection Range")]
    [SerializeField] private float baseRange = 1.0f;

    [Header("SelfDesturct switch waiting time after arrive to the Base")]
    [SerializeField] private float selfDestructDelay = 2.0f;

    [Header("Dead switch waiting time after SelfDestruct")]
    [SerializeField] private float DeadDelay = 4.0f;

    private bool _reachedBasePoint = false;
    private float _reachTimer = 0f;
    private bool _hasTriggeredSelfDestruct = false;
    private float _selfDestructTimer = 0f;


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
            _reachedBasePoint = true;

        if (_hasTriggeredSelfDestruct && (_selfDestructTimer += Time.deltaTime) >= DeadDelay)
            zomnic.Animator.SetTrigger("dead");

        if (_reachedBasePoint && (_reachTimer += Time.deltaTime) >= selfDestructDelay)
        {
            zomnic.Animator.SetTrigger("selfDestruct");
            _hasTriggeredSelfDestruct = true;
        }
    }
}