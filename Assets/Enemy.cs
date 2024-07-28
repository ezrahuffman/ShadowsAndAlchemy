using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Health System
    HealthSystem healthSystem;
    [SerializeField] int maxHealth;
    [SerializeField] string deathAnimationName;
    int deathAnimationHash;
    Animator animator;
    private void Start()
    {
        healthSystem = new HealthSystem(maxHealth);
        healthSystem.onDie += OnDie;
        animator = GetComponent<Animator>();

        deathAnimationHash = Animator.StringToHash(deathAnimationName);

        OverridableStart();
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
