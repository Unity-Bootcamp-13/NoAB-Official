using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zomnic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Vector3 basePoint;

    public NavMeshAgent Agent { get { return agent; } }


    private void OnEnable()
    {
        // To Do : HP reset
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