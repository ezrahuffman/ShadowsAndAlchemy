using UnityEngine;
using UnityEngine.AI;

public class Target : Enemy
{
    NavMeshAgent meshAgent;
    [SerializeField] Transform target;

    protected override void OverridableStart()
    {
        base.OverridableStart();
        meshAgent = GetComponent<NavMeshAgent>();
        MoveToPoint();
    }
    protected override void OnDie()
    {
        base.OnDie();

        // End game
        GameController.instance.OnTargetDie();
    }

    void MoveToPoint()
    {
        meshAgent?.SetDestination(target.position);
    }
}
