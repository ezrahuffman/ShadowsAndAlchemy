using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AttackHitManager : MonoBehaviour
{
    HashSet<Enemy> enemySet;

    private void Awake()
    {
        enemySet = new HashSet<Enemy>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        Debug.Log($"{other.gameObject} entered");
        Enemy enemy = other.GetComponent<Enemy>(); 
        if (enemy != null)
        {
            Debug.Log($"{other.gameObject} can be attacked");
            Debug.Break();
            enemySet.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"{other.gameObject} exited");
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log($"{other.gameObject} cannot be attacked");
            enemySet.Remove(enemy);
        }
    }

    public Enemy GetEnemy()
    {
        if (enemySet.Count == 0)
        {
            return null;
        }
        return enemySet.First();
    }


}
