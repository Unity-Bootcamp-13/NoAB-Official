using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Collider))]
public class Zomnic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Vector3 basePoint;
    [SerializeField] private Animator animator;

    [Header("Zomnic base info")]
    [SerializeField] int maxHp = 200;

    private int _currentHp;
    private ZomnicPoolManager _zomnicPoolManager;

    public Vector3 BasePoint { get { return basePoint; } }
    public int MaxHP { get { return maxHp; } }
    public Animator Animator { get { return animator; } }
    public bool IsDead => _currentHp <= 0;
    public ZomnicPoolManager ZomnicPoolManager { get { return _zomnicPoolManager; } }
    public static event Action<float> OnZomnicDamaged;
    internal bool isSlowed = false;


    private void OnEnable()
    {
        _currentHp = MaxHP;
        animator.Rebind();
        animator.Update(0f);
        isSlowed = false;

        StartCoroutine(MoveToBasePoint());
    }

    private void Update()
    {
        if (isSlowed)
        {
            StartCoroutine(TakeFlashbang());
            isSlowed = false;
        }
    }

    public IEnumerator MoveToBasePoint()
    {
        agent.Warp(transform.position);

        if (agent.isOnNavMesh == false)
            StopCoroutine(MoveToBasePoint());

        yield return null;
        agent.enabled = true;
        agent.SetDestination(basePoint);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        if (_currentHp < damage)
            damage = _currentHp;

        Debug.Log("takedamageµé¾î¿È");
        
        _currentHp -= damage;
        OnZomnicDamaged?.Invoke(damage);
        Debug.Log($"{_currentHp}");
                
        animator.SetBool("isMoving", false);
        animator.SetBool("isSelfDestructing", false);
        animator.SetTrigger("hit");
    }

    public IEnumerator TakeFlashbang()
    {
        float currentSpeed = agent.speed;

        agent.speed = 0;
        yield return new WaitForSeconds(0.9f);
        agent.speed = currentSpeed;
    }

    public void InjectPoolManager(ZomnicPoolManager poolManager)
    {
        _zomnicPoolManager = poolManager;
    }
}