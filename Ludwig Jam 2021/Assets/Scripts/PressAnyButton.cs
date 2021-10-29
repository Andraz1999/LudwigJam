using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class PressAnyButton : MonoBehaviour
{
   [SerializeField] GameObject mainMenu;
    public PlayerInput control;
    public GameObject pauseFirstButton;
    AudioManager audioManager;


    private void Start() {
        //Time.timeScale = 1f;
        control.actions.FindActionMap("Gameplay").Disable();
        control.actions.FindActionMap("PauseMenu").Enable();
        EventSystem.current.SetSelectedGameObject(null);
        audioManager = AudioManager.Instance;
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
    public void AnyButtonInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            control.actions.FindActionMap("PauseMenu").Disable();
            control.actions.FindActionMap("Gameplay").Enable();
            audioManager.Play("playButton");
            Invoke("SetActionMap", 0.1f);
            
        }
    }

    private void SetActionMap()
    {
        
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
}
