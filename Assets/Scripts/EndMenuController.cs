using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuController : MonoBehaviour
{
    [SerializeField] Transform _buttonsParent;
    [SerializeField] GameObject _menuObject;

    [SerializeField] TMP_Text _resultText;
    [SerializeField] string _winningMessage;
    [SerializeField] string _losingMessage;

    private void Awake()
    {
        foreach (Button button in _buttonsParent.GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(() => PlayButtonClick());

            if (button.TryGetComponent(out BasicButton basicButton))
            {
                basicButton.onButtonHover += OnButtonHover;
            }
        }


    }

    void OnButtonHover()
    {
        //TODO: Play button hover sound
    }

    private void PlayButtonClick()
    {
        //TODO: Play button click
    }

    public void OnRestartButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    internal void OpenMenu(bool playerWon)
    {
        _menuObject.SetActive(true);
        Time.timeScale = 0;

        if (playerWon)
        {
            _resultText.text = _winningMessage;
        }
        else
        {
            _resultText.text = _losingMessage;
        }

        //UpdateScores(time);
    }

    //void UpdateScores(float time)
    //{
    //    // Update the high scores
    //    float highScore_1 = PlayerPrefs.GetFloat("HighScore_1", 0);
    //    float highScore_2 = PlayerPrefs.GetFloat("HighScore_2", 0);
    //    float highScore_3 = PlayerPrefs.GetFloat("HighScore_3", 0);

    //    // See if the current score is a high score
    //    if(time > highScore_3 && highScore_3 != 0)
    //    {
    //        SetHighScoreText();
    //        return;
    //    }

    //    if (time < highScore_1 || highScore_1 == 0)
    //    {
    //        PlayerPrefs.SetFloat("HighScore_3", highScore_2);
    //        PlayerPrefs.SetFloat("HighScore_2", highScore_1);
    //        PlayerPrefs.SetFloat("HighScore_1", time);
    //        highScore_1 = time;
    //    }
    //    else if (time < highScore_2 || highScore_2 == 0)
    //    {
    //        PlayerPrefs.SetFloat("HighScore_3", highScore_2);
    //        PlayerPrefs.SetFloat("HighScore_2", time);
    //        highScore_2 = time;
    //    }
    //    else if (time < highScore_3 || highScore_3 == 0)
    //    {
    //        PlayerPrefs.SetFloat("HighScore_3", time);
    //        highScore_3 = time;
    //    }

        
    //    SetHighScoreText();
    //}

    //void SetHighScoreText()
    //{
    //    float hs1 = PlayerPrefs.GetFloat("HighScore_1", 0);
    //    float hs2 = PlayerPrefs.GetFloat("HighScore_2", 0);
    //    float hs3 = PlayerPrefs.GetFloat("HighScore_3", 0);

    //    _highScore_1.text = hs1.ToString("F2") + " s";
    //    _highScore_2.text = hs2.ToString("F2") + " s";
    //    _highScore_3.text = hs3.ToString("F2") + " s";
    //}
}
