using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Health System
    HealthSystem healthSystem;
    [SerializeField] int maxHealth;
    [SerializeField] string deathAnimationName;
    [SerializeField] List<AgentPathPoint> path;
    [SerializeField] float movementSpeed;
    protected NavMeshAgent meshAgent;


    int deathAnimationHash;
    Animator animator;
    int currentPathPointIndex;

    private void Start()
    {
        healthSystem = new HealthSystem(maxHealth);
        healthSystem.onDie += OnDie;
        animator = GetComponent<Animator>();

        deathAnimationHash = Animator.StringToHash(deathAnimationName);

        OverridableStart();

        currentPathPointIndex = 0;
        meshAgent = GetComponent<NavMeshAgent>();
        meshAgent.speed = movementSpeed;

        foreach (AgentPathPoint p in path)
        {
            p.SetAgent(this);
            p.onGoToNextPathPoint += OnGoToNextPathPoint;
        }

        if (path.Count > 0)
        {
            meshAgent.destination = path[0].transform.position;
        }
    }

    protected void OnGoToNextPathPoint(AgentPathPoint point)
    {

        if (path[currentPathPointIndex] != point)
        {
            return;
        }

        currentPathPointIndex = (currentPathPointIndex + 1) % path.Count;
        meshAgent.destination = path[currentPathPointIndex].transform.position;
    }

    protected virtual void OverridableStart()
    {

    }

    public void TakeDmg(int damage)
    {
        Debug.Log("Enemy took dmg");
        healthSystem.TakeDmg(damage);
    }

    protected virtual void OnDie()
    {
        Debug.Log($"{gameObject.name} has died");

        // Play animation
        animator.Play(deathAnimationHash);
    }

    // Movement

}
