using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlayScreen;
    public GameObject mainScreen;
    public GameObject volumeScreen;
    public GameObject settingsScreen;
    public int volSet;
    private void Start() {
        Time.timeScale = 1;
    }
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/tryScene");
        
    }

    public void HowToPlay()
    {
        howToPlayScreen.SetActive(true);
        // mainScreen.SetActive(false);
    }

    public void outOfHowToPlay()
    {
        howToPlayScreen.SetActive(false);
        // mainScreen.SetActive(true);
    }

    public void VolumeScreen()
    {
        if(volSet != 1)
        {
            volumeScreen.SetActive(true);
            settingsScreen.SetActive(false);
            volSet = 1;
            Time.timeScale = 0f;
        }
        else
        {
            volumeScreen.SetActive(false);
            settingsScreen.SetActive(false);
            volSet = 0;
            Time.timeScale = 1f;
        }
    }

    public void SettingsScreen()
    {
        if(volSet != 2)
        {
            volumeScreen.SetActive(false);
            settingsScreen.SetActive(true);
            volSet = 2;
            Time.timeScale = 0f;
        }
        else
        {
            volumeScreen.SetActive(false);
            settingsScreen.SetActive(false);
            volSet = 0;
            Time.timeScale = 1f;
        }
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/MainMenu");
        FindObjectOfType<AudioManager>().Play("MenuTheme");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
