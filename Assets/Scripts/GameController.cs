using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public bool playerCaught { private set; get; }

    bool _gamePaused;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] EndMenuController endMenuController;
    [SerializeField] TMP_Text  promptText;

    Enemy[] enemies;

    bool prompting;

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

        enemies = FindObjectsOfType<Enemy>();
        
        promptText.enabled = false;
    }

    // Show the "Press to use" prompt to the user
    public void PromptUse(string message)
    {
        prompting = true;
        promptText.enabled = true;
        promptText.text = message;
    }

    public void LateUpdate()
    {
        if (!prompting)
        {
            promptText.enabled = false;
        }
        prompting = false;
    }

    internal void OnTargetDie()
    {
        Debug.Log("Game over! Show the winning end screen.");

        endMenuController.OpenMenu(playerWon: true);
    }

    internal void OnPlayerCaught()
    {
        Debug.Log("Player caught. Game Over");
        playerCaught = true;

        // Show Game Over Screen as lost
        endMenuController.OpenMenu(playerWon: false);
    }

    public void PauseEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.isActiveAndEnabled)
            {
                enemy.canMove = false;
            }
        }
    }

    public void UnPauseEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.isActiveAndEnabled && !enemy.isDead)
            {
                enemy.canMove = true;
            }
        }
    }

    internal void PauseGame()
    {
        Time.timeScale = 0;
        _gamePaused = true;
    }

    internal void ResumeGame()
    {
        Time.timeScale = 1;
        _gamePaused = false;
    }

    internal void TogglePause()
    {
        _gamePaused = !_gamePaused;
        if (_pauseMenu != null)
        {
            _pauseMenu?.SetActive(_gamePaused);
        }
        if (_gamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }
}
