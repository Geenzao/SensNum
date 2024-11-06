using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class MainMenu : Menu
{
    [SerializeField] private string levelToLoad;

    [Header("Boutons")]
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonSettings;

    [Header("Panel")]
    [SerializeField] private GameObject panelSettings;

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
    }

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
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
        GameManager.Instance.LoadLevel(levelToLoad);
    }

    private void OnSettingsButtonClicked()
    {
        panelSettings.SetActive(!panelSettings.activeSelf);
    }

    private void DesactivateAllPanel()
    {
        panelSettings.SetActive(false);
    }
}
