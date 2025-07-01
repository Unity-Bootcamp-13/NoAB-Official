using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
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
    public int CurrentHp { get { return _currentHp; } }
    public Animator Animator { get { return animator; } }
    public bool IsDead => _currentHp <= 0;
    public ZomnicPoolManager ZomnicPoolManager { get { return _zomnicPoolManager; } }


    private void OnEnable()
    {
        _currentHp = MaxHP;
        MoveToBasePoint();
    }

    public void MoveToBasePoint()
    {
        agent.Warp(transform.position);

        if (agent.isOnNavMesh == false)
            return;
        agent.SetDestination(basePoint);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        _currentHp -= damage;
        animator.SetTrigger("hit");
    }

    public void InjectPoolManager(ZomnicPoolManager poolManager)
    {
        _zomnicPoolManager = poolManager;
    }
}