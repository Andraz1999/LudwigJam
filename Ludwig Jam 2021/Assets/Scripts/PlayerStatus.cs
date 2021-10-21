using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        healthbar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        timer = TimerController.Instance;
        progressbar = Progressbar.Instance;
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
            if (damage < 0)
            {
                SetInvincibility();
            }
            if (currentHealth <= 0)
            {
                Die();
            }   
        
        }   
    }

    public void Die()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(currentHealth);
        player.Respawn();
        progressbar.ResetProgress();
        Debug.Log(timer.GetTime());
        timer.RestartTimer();
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
