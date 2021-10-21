using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public PlayerInput control;

    public static bool gameIsPaused = false;
    public static bool quitMenuActive = false;
    public GameObject pauseMenuUI;
    public GameObject quitMenuUI;

    public GameObject pauseFirstButton, quitFirstButton, quitClosedButton;

    
    public void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(gameIsPaused) 
            {
                if (quitMenuActive)
                {
                    QuitMenu();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }  
    }
    
    public void Resume()
    {
        control.actions.FindActionMap("PauseMenu").Disable();
        control.actions.FindActionMap("Gameplay").Enable();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    void Pause() 
    {

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        control.actions.FindActionMap("Gameplay").Disable();
        control.actions.FindActionMap("PauseMenu").Enable();
    }

    public void Restart()
    {
        Debug.Log("You want to restart");
    }

    public void QuitMenu()
    {
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
    }

    public void Quit()
    {
        Debug.Log("You want to go to menu");
    }

}
