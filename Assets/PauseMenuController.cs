using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject _settingsMenu;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Transform _buttonsParent;


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

    public void OnResumeButtonClick()
    {
        _pauseMenu.SetActive(false);
        GameController.instance.ResumeGame();
    }

    public void OnRestartButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnSettingsButtonClick()
    {
        SwitchMenu(_pauseMenu, _settingsMenu);
    }

    public void OnMainMenuButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void SwitchMenu(GameObject currMenu, GameObject nextMenu)
    {
        currMenu.SetActive(false);
        nextMenu.SetActive(true);
    }


}
