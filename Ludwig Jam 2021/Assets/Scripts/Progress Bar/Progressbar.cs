using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    
    [SerializeField] Slider slider;
    [SerializeField] int maxProgress;
    private int currentProgress;

    #region Singleton
    public static Progressbar Instance {get; private set;}
    
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
        slider.maxValue = maxProgress;
        ResetProgress();
    }

    public void SetProgress(int progress)
    {
        slider.value = progress;
    }
    public void ResetProgress()
    {
        SetProgress(0);
    }

}
