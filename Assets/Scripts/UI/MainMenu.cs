using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class MainMenu : Menu
{
    [SerializeField] private string levelToLoad;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI titre;
    [SerializeField] private TextMeshProUGUI play;
    [SerializeField] private TextMeshProUGUI settings;

    [Header("Boutons")]
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonSettings;

    [Header("Panel")]
    [SerializeField] private GameObject panelGeneral;
    [SerializeField] private GameObject panelSettings;

    private void Awake()
    {
        LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.MainMenu)
        {
            TriggerVisibility(true);
        }

        buttonPlay.onClick.AddListener(OnPlayButtonClicked);
        buttonSettings.onClick.AddListener(OnSettingsButtonClicked);

        DesactivateAllPanel();

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
        panelGeneral.SetActive(visible);
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.MainMenu)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.UnloadLevel("Start");
        GameManager.Instance.LoadLevel(levelToLoad);
        GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.Start);
    }

    private void OnSettingsButtonClicked()
    {
        panelSettings.SetActive(!panelSettings.activeSelf);
    }

    private void DesactivateAllPanel()
    {
        panelSettings.SetActive(false);
    }

    private void UpdateTexts()
    {
        if (titre == null || play == null || settings == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;
        }

        titre.text = LanguageManager.Instance.GetText("MainMenu_Title");
        play.text = LanguageManager.Instance.GetText("play");
        settings.text = LanguageManager.Instance.GetText("settings");
    }
}
