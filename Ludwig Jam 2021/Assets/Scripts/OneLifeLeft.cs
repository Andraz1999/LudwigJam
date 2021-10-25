using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OneLifeLeft : MonoBehaviour
{
    public GameObject dangerScreen;
    public PlayerInput control;
    bool gameIsPaused;
    public GameObject pauseFirstButton;

    #region Singleton
    public static OneLifeLeft Instance {get; private set;}
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }   
    #endregion


    public void Danger()
    {   
        Time.timeScale = 0f;
        gameIsPaused = true;
        control.actions.FindActionMap("Gameplay").Disable();
        //control.actions.FindActionMap("Danger").Enable();
        dangerScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void ContinueGame()
    {
        if(gameIsPaused)
        {
            dangerScreen.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
            //control.actions.FindActionMap("Danger").Disable();
            control.actions.FindActionMap("GamePlay").Enable();
            
        }
    }
    public void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed && gameIsPaused)
        {
            ContinueGame();
        }
    }

}
