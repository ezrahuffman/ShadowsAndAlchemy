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

    [SerializeField] List<AudioClip> stabSounds;
    [SerializeField] AudioSource audioSource;
    int prevStabSoundIndex;

    public bool canMove { 
        get { return meshAgent.speed > 0f; }
        set {
            if (meshAgent == null)
            {
                return;
            }
            if (value)
            {
                // can move
                
                meshAgent.speed = movementSpeed;
            }
            else
            {
                meshAgent.speed = 0f;
            }
        } 
    }
    public bool isDead { get; private set; }

    private void Start()
    {
        canMove = true;
        prevStabSoundIndex = -1; // allow any stab sound to be played first

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

        PlayStabSound();
    }

    void PlayStabSound()
    {
        int randIndex = Random.Range(0, stabSounds.Count);
        if (randIndex == prevStabSoundIndex)
        {
            randIndex = (randIndex + 1) % stabSounds.Count;
        }
        audioSource.clip = stabSounds[randIndex];
        prevStabSoundIndex = randIndex;
        audioSource.Play();  
    }


    protected virtual void OnDie()
    {
        Debug.Log($"{gameObject.name} has died");

        isDead = true;

        canMove = false;

        // Play animation
        animator.Play(deathAnimationHash);
    }

    // Movement

}
