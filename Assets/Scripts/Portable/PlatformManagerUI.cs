using UnityEngine;
using static UnityEngine.AudioSettings;

public class PlatformManagerUI : Menu
{

    [SerializeField] private GameObject mobileJoystick; // Assigner ici le joystick UI
    public Joystick joystick;

    private bool isMobile = false;

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        mobileJoystick.SetActive(visible);
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.None && isMobile)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    void Start()
    {
        base.Start();

        PlatformManager.Instance.DetectPlatform();
        isMobile = PlatformManager.Instance.fctisMobile();

        if (UIManager.CurrentMenuState == UIManager.MenuState.None && isMobile)
        {
            TriggerVisibility(true);
        }


        if(isMobile)
            Debug.Log("On affiche le joystick");
        else
            Debug.Log("On n'affiche PAS le joystick");
    }

    private void Update()
    {
        if(UIManager.CurrentMenuState == UIManager.MenuState.None && playerMovement.Instance != null)
            playerMovement.Instance.setMove(joystick.Horizontal, joystick.Vertical);
    }
}
