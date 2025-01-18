using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("Game stats:")]
    public bool isPause = false;

    //[SerializeField] private GameObject mainMenu;
    //[SerializeField] private GameObject gamePlayMenu;
    [SerializeField] private GameObject gamePauseMenu;
    public void GameStart()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
    public void GameExit()
    {
        Application.Quit();
    }
    public void GamePause() 
    {
        Time.timeScale = 0f;
        gamePauseMenu.SetActive(true);
        isPause = true;
    }
    public void GameResume() 
    {
        Time.timeScale = 1f;
        gamePauseMenu.SetActive(false);
        isPause = false;
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitToMenu()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }
}
