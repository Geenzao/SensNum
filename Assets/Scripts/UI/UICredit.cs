using UnityEngine;
using UnityEngine.UIElements;

public class UICredit : Menu
{
    [SerializeField] public GameObject creditPanel;
    [SerializeField] public GameObject text;
    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.Credits)
        {
            TriggerVisibility(true);
        }
    }

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        //Start couroutine de 2 seconde
        if (visible)
        {
            creditPanel.SetActive(visible);
            text.SetActive(visible);
        }
        else
        {
            creditPanel.SetActive(false);
            text.SetActive(false);
        }
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.Credits)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }
}
