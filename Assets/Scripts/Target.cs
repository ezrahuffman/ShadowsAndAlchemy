using UnityEngine;
using UnityEngine.AI;

public class Target : Enemy
{
    //NavMeshAgent meshAgent;
    [SerializeField] Transform target;

    protected override void OverridableStart()
    {
        base.OverridableStart();
        //meshAgent = GetComponent<NavMeshAgent>();
        //MoveToPoint();
    }

    void MoveToPoint()
    {
        meshAgent?.SetDestination(target.position);
    }
}
