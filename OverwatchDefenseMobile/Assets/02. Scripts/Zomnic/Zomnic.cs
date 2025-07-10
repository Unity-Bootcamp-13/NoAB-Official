using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Collider))]
public class Zomnic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Vector3 basePoint;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private AudioSource hurtSound;

    public Transform GetHeadTransform() => headTransform;

    private int _maxHp = 450;
    private int _currentHp;
    private ZomnicPoolManager _zomnicPoolManager;
    private bool isFirstDamaged = true;

    public Vector3 BasePoint { get { return basePoint; } }
    public int MaxHP { get { return _maxHp; } }
    public int CurrentHP { get { return _currentHp; } }
    public Animator Animator { get { return animator; } }
    public bool IsDead => _currentHp <= 0;
    public ZomnicPoolManager ZomnicPoolManager { get { return _zomnicPoolManager; } }
    public static event Action<float> OnZomnicDamaged;
    internal bool isSlowed = false;


    private void OnEnable()
    {
        isFirstDamaged = true;
        hpSlider.gameObject.SetActive(false);
        _currentHp = _maxHp;
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

        hpSlider.maxValue = _maxHp;
        hpSlider.value = _currentHp;
        hpSlider.transform.forward = Camera.main.transform.forward;
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

        if (isFirstDamaged)
        {
            isFirstDamaged = false;
            hpSlider.gameObject.SetActive(true);
        }

        hurtSound.Play();

        if (_currentHp < damage)
            damage = _currentHp;        
        
        _currentHp -= damage;
        OnZomnicDamaged?.Invoke(damage);
        
        if (_currentHp <= 0)
            hpSlider.gameObject.SetActive(false);
                
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