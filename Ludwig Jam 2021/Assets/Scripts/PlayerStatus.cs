using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    PlayerMovement player;

    [Header("Health")]
    [SerializeField] int maxHealth;
    Healthbar healthbar;
    private int currentHealth;
    [Header("Invincibility")]
    private bool invincibility;
    [SerializeField] float invincibilityTime = 10f;
    [SerializeField] private float invincibilitymaxcount = 0.2f;
    private float invincibilitycount;
    [SerializeField] private Renderer playerSprite;
    private TimerController timer;
    private Progressbar progressbar;

    private PauseMenu pauseMenu;
    private OneLifeLeft oneLifeLeft;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] UnityEvent onRespawn;

    private AudioManager audioManager;

    #region Singleton
    public static PlayerStatus Instance {get; private set;}
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }   
    #endregion

    void Start()
    {
        player = PlayerMovement.Instance;
        healthbar = Healthbar.Instance;
        healthbar.SetMaxHealth(120);
        healthbar.SetHealth(maxHealth);
        currentHealth = maxHealth;
        timer = TimerController.Instance;
        progressbar = Progressbar.Instance;
        pauseMenu = PauseMenu.Instance;
        oneLifeLeft = OneLifeLeft.Instance;

        audioManager = AudioManager.Instance;

    }

    
    private void Update() 
    { 
        if(invincibility)
        {
            // handling turn on and turn off of the render
            invincibilitycount -= Time.deltaTime;
            if (invincibilitycount <= 0)
            {
                playerSprite.enabled = !playerSprite.enabled;
                invincibilitycount = invincibilitymaxcount;
            }
        }
    } 

    public void Health(int damage, bool forceDamage = false)
    {
        if(!invincibility || forceDamage)
        {
            int health = currentHealth + damage;
            healthbar.SetHealth(health);
            currentHealth = healthbar.GetHealth();
            audioManager.Play("hurt");
            if (damage < 0)
            {
                SetInvincibility();
            }
            if (currentHealth <= 0)
            {
                //Die();
                
                pauseMenu.Pause(false);
            }   
            if(currentHealth == 10)
            {
                oneLifeLeft.Danger();
            }
        
        }   
    }

    public void Die()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(currentHealth);
        progressbar.ResetProgress();
        timer.RestartTimer();
        player.Respawn();
        cam.Priority = 20;
        onRespawn.Invoke();
        DeactivateInvincibility();
    }

    public void Goal()
    {
        Time.timeScale = 0f;
        pauseMenu.GoalScene(timer.GetTimeString());
    }


    private void SetInvincibility()
    {
        invincibility = true;
        invincibilitycount = invincibilitymaxcount;
        Invoke("DeactivateInvincibility", invincibilityTime);
    }
    private void DeactivateInvincibility()
    {
        invincibility = false;
        playerSprite.enabled = true;
    }

}
