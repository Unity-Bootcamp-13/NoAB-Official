using System.Collections.Generic;
using UnityEngine;

public class ProjectileTarget : MonoBehaviour
{
    public List<Collider> targetList;

    private void Awake()
    {
        targetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!targetList.Contains(other) && other.CompareTag("Zomnic"))
            targetList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetList.Contains(other) && other.CompareTag("Zomnic"))
            targetList.Remove(other);
    }

    private void LateUpdate()
    {
        targetList.RemoveAll(target => target == null);
    }
}
