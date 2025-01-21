using System;
using UnityEngine;
using static GameProgressManager;

public class ChargementTransition : Menu
{
    public Animator animator;
    public GameObject LoadGreenRoue;

    [SerializeField] private GameObject panelChargement; 

    private void Awake()
    {
        LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.Loading)
        {
            Debug.Log("CandyCrush");
            TriggerVisibility(true);
        }

        if (LanguageManager.Instance != null)
        {
            UpdateTexts();
        }
        else
        {
            Debug.LogError("LanguageManager instance is not initialized.");
        }
    }

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        //Start couroutine de 2 seconde
        if (visible)
        {
            panelChargement.SetActive(true);
            LoadChargement();
        }
        else
        {
            panelChargement.SetActive(false);
        }
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.Loading)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    public void LoadChargement()
    {
        Debug.Log("LoadChargement");
        animator.SetTrigger("LoadPage");
        print("LoadPage trigger est fini");
    }

    public void StopGame()
    {
        ChargementTransitionManager.Instance.StopGame();
    }

    public void StartGame()
    {
        ChargementTransitionManager.Instance.StartGame();
    }

    private void UpdateTexts()
    {
        //if ()
        //{
        //    Debug.LogError("Text elements are not assigned in the inspector.");
        //    return;
        //}
    }
}
