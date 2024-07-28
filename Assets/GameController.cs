using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance =  this;
        }
        else
        {
            Debug.LogError("There is already a game controller but you are trying to create a new one.");
            Destroy(gameObject);
            return;
        }
    }

    // Show the "Press to use" prompt to the user
    public void PromptUse()
    {
        Debug.Log("Show use prompt");
    }

    internal void OnTargetDie()
    {
        Debug.Log("Game over! Show the winning end screen.");
    }
}
