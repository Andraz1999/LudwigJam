using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlayScreen;
    public GameObject storyScreen;
    public GameObject mainScreen;
    public GameObject volumeScreen;
    public GameObject settingsScreen;
    public int volSet;

    public GameObject[] htpScrens;
    int htpScr = 0;

    public GameObject[] storyScrens;
    int storyScr = 0;
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
        htpScr = 0;
        nextHowToPlayScrene();
        // mainScreen.SetActive(false);
    }
    public void Story()
    {
        storyScreen.SetActive(true);
        storyScr = 0;
        nextStoryScrene();
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

    public void nextHowToPlayScrene()
    {
        int lastHtpScr = htpScr;
        if(htpScr == htpScrens.Length - 1)
        {
            SetHowToPlayScrene(lastHtpScr, 0);
            howToPlayScreen.SetActive(false);
            return;
        }
        else
            htpScr += 1;


        SetHowToPlayScrene(lastHtpScr, htpScr);
    }

    public void lastHowToPlayScrene()
    {
        int lastHtpScr = htpScr;
        if(htpScr == 1)
        {
            howToPlayScreen.SetActive(false);
            return;
        }
        else
            htpScr -= 1;

        SetHowToPlayScrene(lastHtpScr, htpScr);
    }

    void SetHowToPlayScrene(int lastHtpScr, int scr)
    {
        htpScrens[scr].SetActive(true);
        htpScrens[lastHtpScr].SetActive(false);

    }

    public void nextStoryScrene()
    {
        int lastStoryScr = storyScr;
        if(storyScr == htpScrens.Length - 1)
        {
            SetStoryScrene(lastStoryScr, 0);
            storyScreen.SetActive(false);
            return;
        }
        else
            storyScr += 1;


        SetStoryScrene(lastStoryScr, storyScr);
    }

    public void lastStoryScrene()
    {
        int lastStoryScr = storyScr;
        if(storyScr == 1)
        {
            storyScreen.SetActive(false);
            return;
        }
        else
            storyScr -= 1;

        SetStoryScrene(lastStoryScr, storyScr);
    }

    void SetStoryScrene(int lastStoryScr, int scr)
    {
        storyScrens[scr].SetActive(true);
        storyScrens[lastStoryScr].SetActive(false);

    }
}
