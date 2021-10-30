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
    [SerializeField] CinemachineVirtualCamera camNarrow;
    [SerializeField] CinemachineVirtualCamera camWide;
    //[SerializeField] UnityEvent onRespawn;

    private AudioManager audioManager;
    ////////// saving
    [Header("Saving")]
    SaveManager saveManager;
    public float time;
    public int checkpoint;
    public float bestTime;
    public string bestTimeString;
    public CheckPoint[] checkPoints;

    

    #region Singleton
    public static PlayerStatus Instance {get; private set;}
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        checkpoint = -1;
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
        saveManager = SaveManager.Instance;

        
        // 

        StartCoroutine(LateStart(0.01f));
     }
 
     IEnumerator LateStart(float waitTime)
     {
         yield return new WaitForSeconds(waitTime);
            saveManager.Load();
            Respawn();
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
                timer.EndTimer();
                time = timer.GetTime();
                saveManager.Save();
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
        //timer.RestartTimer();
        //player.Respawn();
        //cam.Priority = 20;
        //onRespawn.Invoke();
        DeactivateInvincibility();
    }

    public void Goal()
    {
        // Time.timeScale = 0f;
        if (bestTimeString.Equals(""))
        {
            bestTime = timer.GetTime();
            bestTimeString = timer.GetTimeString();
        }
        else if (timer.GetTime() < bestTime)
        {
            bestTime = timer.GetTime();
            bestTimeString = timer.GetTimeString();
        }
        checkpoint = -1;
        time = 0;
        saveManager.Save();
        pauseMenu.GoalScene(timer.GetTimeString(), bestTimeString);
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

    private void Respawn()
    {
        Debug.Log("respawn 1");
        if(checkpoint == -1)
        {
            Debug.Log("respawn 2");
            player.Respawn(new Vector3(0f, -0.9f, 0), false);
            progressbar.ResetProgress();
            timer.BeginTimer();
        }
        else
        {
            Debug.Log("respawn 2.1");
            player.Respawn(checkPoints[checkpoint].gameObject, checkPoints[checkpoint].startSwitch);
            Debug.Log("player is resawned successfully");
            timer.BeginTimer(time);
            Debug.Log("time is set");
            if(checkPoints[checkpoint].isCameraWide)
            {
                camNarrow.Priority = 5;
                camWide.Priority = 10;
                Debug.Log("camera is now wide");
            }
            else
            {
                camNarrow.Priority = 10;
                camWide.Priority = 5;
                Debug.Log("camera is now narrow");
            }
            checkPoints[checkpoint].isActive = false;
            progressbar.SetProgress(checkPoints[checkpoint].progress);
            checkpoint--;
            Debug.Log("everything done");
        }
    }
    public void Restart()
    {
        checkpoint = -1;
        time = 0;
        saveManager.Save();
    }

}
