using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zomnic : Enemy
{
    [SerializeField] private NavMeshAgent agent;

    public NavMeshAgent Agent { get { return agent; } }

    public void MoveToBasePoint(Vector3 basePoint)
    {
        agent.SetDestination(basePoint);
    }
}