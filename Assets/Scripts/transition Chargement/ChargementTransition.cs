using System;
using TMPro;
using UnityEngine;
using static GameProgressManager;

public class ChargementTransition : Menu
{
    public Animator animator;
    public GameObject LoadGreenRoue;

    [SerializeField] private GameObject panelChargement;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI txtLoading1;
    [SerializeField] private TextMeshProUGUI txtLoading2;
    [SerializeField] private TextMeshProUGUI txtLoading3;
    [SerializeField] private TextMeshProUGUI txtLoading4;

    private void Awake()
    {
        LanguageManager.OnLanguageChanged += UpdateTexts;
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.Loading)
        {
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
        animator.SetTrigger("LoadPage");
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
        if (txtLoading1 == null || txtLoading2 == null || txtLoading3 == null || txtLoading4 == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;
        }
        txtLoading1.text = LanguageManager.Instance.GetText("loading");
        txtLoading2.text = LanguageManager.Instance.GetText("loading") + ".";
        txtLoading3.text = LanguageManager.Instance.GetText("loading") + "..";
        txtLoading4.text = LanguageManager.Instance.GetText("loading") + "...";
    }
}
