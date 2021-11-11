using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public PlayerInput control;
    PlayerStatus playerStatus;

    [Header("Menu")]
    public static bool gameIsPaused = false;
    public static bool quitMenuActive = false;
    public GameObject pauseMenuUI;
    public GameObject quitMenuUI;

    public GameObject pauseFirstButton, quitFirstButton, quitClosedButton;

    bool canResume;

    // [Header("Game Over")]
     public GameObject gameOverUI;
     public GameObject quitGameOverUI;
    public GameObject gameOverFirstButton, gameOverQuitFirstButton, gameOverQuitClosedButton;
    ////////////////
    public GameObject tab1, tab2;

    ///////
    public GameObject goalScreen;
    public GameObject goalScreenButton;
    public TextMeshProUGUI goalText, recordText;

    #region Singleton
    public static PauseMenu Instance {get; private set;}
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }   
    #endregion


    private void Start() 
    {
        playerStatus = PlayerStatus.Instance;  
        control.actions.FindActionMap("PauseMenu").Disable(); 
        control.actions.FindActionMap("Gameplay").Enable();
        gameIsPaused = false;
        quitMenuActive = false;
         
    }

    public void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("pressewd menu");
            if(gameIsPaused) 
            {
                if (quitMenuActive)
                {
                    //Debug.Log("should quit");
                    QuitMenu();
                }
                else if(canResume)
                {
                    //Debug.Log("should resume");
                    Resume();
                }
            }
            else
            {
                Debug.Log("should pause");
                Pause(true);
            }
        }  
    }
    
    public void Resume()
    {
        control.actions.FindActionMap("PauseMenu").Disable();
        control.actions.FindActionMap("Gameplay").Enable();
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void Pause(bool canResume) 
    {
        playerStatus.SaveStatus();
        this.canResume = canResume;
        Time.timeScale = 0f;
        gameIsPaused = true;
        control.actions.FindActionMap("Gameplay").Disable();
        control.actions.FindActionMap("PauseMenu").Enable();
        
        if(canResume)
        {
            pauseMenuUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);

            
        }
        else
        {
            gameOverUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameOverFirstButton);

            
        }
    }

    public void RestartPause()
    {
        // playerStatus.Die();
        //Resume();
        Time.timeScale = 1f;
        
       
       playerStatus.Restart();
       SceneManager.LoadScene("Scenes/Game");
    }
    public void RestartDie()
    {
        // playerStatus.Die();
        //Resume();
        Time.timeScale = 1f;
       SceneManager.LoadScene("Scenes/Game");
    }

     

    public void QuitMenu()
    {   
        if(canResume)
            if (quitMenuActive)
            {
                quitMenuUI.SetActive(false);
                quitMenuActive = false;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(quitClosedButton);
            }
            else 
            {
                quitMenuUI.SetActive(true);
                quitMenuActive = true;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(quitFirstButton);
            }
        else
        {
            if (quitMenuActive)
            {
                quitGameOverUI.SetActive(false);
                quitMenuActive = false;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(gameOverQuitClosedButton);
            }
            else 
            {
                quitGameOverUI.SetActive(true);
                quitMenuActive = true;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(gameOverQuitFirstButton);
            }
        }
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().StopDoublePart();
        FindObjectOfType<AudioManager>().Play("MainMenu");
        SceneManager.LoadScene("Scenes/MainMenu");
        
    }

    public void SwitchTabs(bool isSwitched)
    {
        if(isSwitched)
        {
            tab1.SetActive(false);
            tab2.SetActive(true);
        }
        else
        {
            tab2.SetActive(false);
            tab1.SetActive(true);
        }
    }

    public void GoalScene(string text, string record)
    {
        control.actions.FindActionMap("Gameplay").Disable();
        goalScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(goalScreenButton);
        goalText.SetText(text);
        recordText.SetText(record);
    }

}
