using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int dmg;
    [SerializeField] string animationName;
    [SerializeField] int durability;
    [SerializeField] AttackHitManager hitManager;
    [SerializeField] Sprite icon;
    

    int attackAnimationHash;
    int _remainingDurability;

    public delegate void OnBreak(Weapon weapon);
    public OnBreak onBreak;
   

    private void Start()
    {
        attackAnimationHash = Animator.StringToHash(animationName);
        _remainingDurability = durability;
    }

    private void OnEnable()
    {
        hitManager.gameObject.SetActive(true);
        GameController.instance.SetWeaponIcon(icon);
    }

    private void OnDisable()
    {
        hitManager.gameObject.SetActive(false);
    }

    public void Attack(Animator animator)
    {
        Debug.Log("Weapon Attack");

        animator.Play(attackAnimationHash); // This needs to also be handled by the mage controller so other animations don't cancel it

        _remainingDurability--;

        GameController.instance.SetDurabilityFill((float)_remainingDurability / durability);

        if (_remainingDurability <= 0)
        {
            BreakWeapon();
        }

        Enemy hitEnemy = hitManager.GetEnemy();
        if (hitEnemy != null)
        {
            hitEnemy.TakeDmg(dmg);
        }
        else
        {
            Debug.Log("Didn't hit anything with attack");
        }
    }

    void BreakWeapon()
    {
        onBreak?.Invoke(this);
    }
}
