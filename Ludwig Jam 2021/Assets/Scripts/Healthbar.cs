using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI text;

    #region Singleton
    public static Healthbar Instance {get; private set;}
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }   
    #endregion


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
        text.SetText(GetHealth().ToString() + "%");
    }

    public int GetHealth()
    {
        return (int) slider.value;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        text.SetText(GetHealth().ToString() + "%");
    }
}
