using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using static UnityEngine.AudioSettings;

public class PlatformManagerUI : Menu
{

    [SerializeField] private GameObject mobileJoystick; // Assigner ici le joystick UI
    public Joystick joystick;
    public Button btnPause;
    public GameObject btnPauseUI; //pour le cacher ou non

    private bool isMobile = false;

    private void Awake()
    {
        btnPause.onClick.AddListener(OnPauseButtonClicked);
        ChargementTransitionManager.OnResetJoystick += ResetPosJoystick;
    }

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        mobileJoystick.SetActive(visible);
        btnPauseUI.SetActive(visible);
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
        {
            return;
        }
            /*Debug.Log("On affiche le joystick");*/
        else
        {
            return;
        }
            /*Debug.Log("On n'affiche PAS le joystick");*/

        //On ne fait pas de snap  avec les joystick
        joystick.SnapX = true;
        joystick.SnapY = true;
    }

    private void OnPauseButtonClicked()
    {
        if (dialogueManager.Instance.fctisDialogueActive() == true)
            return;

        if (GameManager.CurrentGameState == GameState.RUNNING && UIManager.CurrentMenuState == UIManager.MenuState.None)
        {
            GameManager.Instance.UpdateGameState(GameState.PAUSED);
        }
        else if (GameManager.CurrentGameState == GameState.PAUSED)
        {
            GameManager.Instance.UpdateGameState(GameState.RUNNING);
        }
    }

    private void Update()
    {
        if (isMobile  && UIManager.CurrentMenuState == UIManager.MenuState.None && playerMovement.Instance != null)
            playerMovement.Instance.setMove(joystick.Horizontal, joystick.Vertical);
    }

    public void ResetPosJoystick()
    {
        joystick.ResetJoystick();
    }
}
