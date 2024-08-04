
using UnityEngine;

public class HealthSystem
{
    int _maxHealth;
    int _currHealth;

    public delegate void OnDie();
    public OnDie onDie;

    public HealthSystem(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currHealth = maxHealth;
    }

    public void TakeDmg(int amount)
    {
        _currHealth -= amount;

        if (_currHealth < 0)
        {
            _currHealth = 0;
        }

        if (_currHealth == 0 )
        {
            Die();
        }
    }

    void Die()
    {
        onDie?.Invoke();
    }


    #region Getters

    public int CurrHealth
    {
        get { 
            return _currHealth;
        }
    }

    public int MaxHealth
    { 
        get 
        { 
            return _maxHealth; 
        } 
    }

    #endregion
}
