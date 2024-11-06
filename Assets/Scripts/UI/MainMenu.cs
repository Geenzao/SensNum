using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class MainMenu : Menu
{

    //Tout les boutons du menu principal:
    [Header("Boutons")]
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonSettings;

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.MainMenu)
        {
            TriggerVisibility(true);
        }

        //buttonContinue.onClick.AddListener(OnContinueButtonClicked);
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
}
