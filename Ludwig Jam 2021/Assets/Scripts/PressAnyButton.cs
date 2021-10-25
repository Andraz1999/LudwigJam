using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PressAnyButton : MonoBehaviour
{
   [SerializeField] GameObject mainMenu;
    public PlayerInput control;


    private void Start() {
        //Time.timeScale = 1f;
        control.actions.FindActionMap("Gameplay").Disable();
        control.actions.FindActionMap("PauseMenu").Enable();
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
    public void AnyButtonInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            control.actions.FindActionMap("PauseMenu").Disable();
            control.actions.FindActionMap("Gameplay").Enable();
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
            
        }
    }
}
