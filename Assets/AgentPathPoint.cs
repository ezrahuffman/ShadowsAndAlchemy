using System;
using UnityEngine;

public class AgentPathPoint : MonoBehaviour
{
    [SerializeField] float timeToWait;

    public delegate void OnGoToNextPathPoint(AgentPathPoint point);
    public OnGoToNextPathPoint onGoToNextPathPoint;

    Enemy agent;

    internal void SetAgent(Enemy enemy)
    {
        agent = enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) // Avoid interactions with the sphere collider on the guards
        {
            return;
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null && enemy == agent)
        {
            if (other.GetComponent<Guard>() != null)
            {
                Debug.Log("start timer");
            }
            StartWaitTimer();
        }
    }

    void StartWaitTimer()
    {
        Invoke("InvokeEvent", timeToWait);
    }

    void InvokeEvent()
    {
        Debug.Log("go to next Point");
        onGoToNextPathPoint?.Invoke(this);
    }
}
