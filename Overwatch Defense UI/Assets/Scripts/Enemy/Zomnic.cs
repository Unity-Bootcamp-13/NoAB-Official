using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zomnic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Vector3 basePoint;

    [Header("Zomnic base info")]
    [SerializeField] int hp = 200;

    public int HP { get { return hp; } }

    private void OnEnable()
    {
        hp = 200;
        MoveToBasePoint();
    }

    public void MoveToBasePoint()
    {
        agent.Warp(transform.position);

        if (agent.isOnNavMesh == false)
            return;
        agent.SetDestination(basePoint);
    }
}