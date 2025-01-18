using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Game stats:")]
    public bool isPause = false;

    public Slider musicSlider, sfxSlider;

    [SerializeField] private GameObject gamePauseMenu;


    private void Awake()
    {
        SetMusic(PlayerPrefs.GetFloat("Music"));
        SetSfx(PlayerPrefs.GetFloat("Sfx"));
    }

    //Scene Controller
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
    //Audio Controller
    public void ToggleMusic()
    {
        SoundManager.instance.ToggleMusic();
    }
    public void ToggleSfx()
    {
        SoundManager.instance.ToggleSfx();
    }
    public void MusicVolume()
    {
        SoundManager.instance.MusicVolume(musicSlider.value);
    }
    public void SetMusic(float volume)
    {
        musicSlider.value = volume; 
    }
    public void SetSfx(float volume)
    {
        sfxSlider.value = volume;
    }
    public void SfxVolume()
    {
        SoundManager.instance.SfxVolume(sfxSlider.value);
    }
}
