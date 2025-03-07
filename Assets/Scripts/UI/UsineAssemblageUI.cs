using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;



/*
 *
 *A voir si on ne fait pas un seul canvas pour tous les minu jeux avec la même DA ou un par mini jeux
 *
 *
 *
 */

public enum UsineAssemblageState
{
    rule, //moment où on explique les regles
    game, //moment ou on joue
    menu //moment où il y a un menu win ou lose d'afficher
}

public class UsineAssemblageUI : Menu
{
    //Pour l'UI
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreNumberLoose;
    [SerializeField] private TextMeshProUGUI scoreNumberWin;
    public TextMeshProUGUI txtRules;
    public TextMeshProUGUI txtWin;
    public TextMeshProUGUI txtLose;
    public TextMeshProUGUI txtReplayLose;
    public TextMeshProUGUI txtQuitLose;
    public TextMeshProUGUI txtReplayWin;
    public TextMeshProUGUI txtQuitWin;
    public TextMeshProUGUI txtNotifyAcceleration;

    public TextMeshProUGUI txtNbCircuitWin;
    public TextMeshProUGUI txtTime;

    [Header("Panel")]
    public GameObject PanelRuler;
    public GameObject PanelInformation;
    public GameObject PanelWin;
    public GameObject PanelLose;
    public GameObject PanelNotifyAcceleration;

    [Header("Button")]
    public Button btnReplayWin;
    public Button btnReplayLose;
    public Button btnQuitWin;
    public Button btnQuitLose;

    private int _time = 0;
    private UsineAssemblageState state;

    private void Awake()
    {
        LanguageManager.OnLanguageChanged += UpdateTexts;
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.AssemblyGame)
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

        btnReplayWin.onClick.AddListener(OnReplayButtonClicked);
        btnReplayLose.onClick.AddListener(OnReplayButtonClicked);
        btnQuitWin.onClick.AddListener(OnQuitButtonClicked);
        btnQuitLose.onClick.AddListener(OnQuitButtonClicked);
    }

    protected override void TriggerVisibility(bool visible)
    {
        UpdateTexts();
        base.TriggerVisibility(visible);
        PanelRuler.SetActive(visible);
        PanelInformation.SetActive(false);
        PanelWin.SetActive(false);
        PanelLose.SetActive(false);
        PanelNotifyAcceleration.SetActive(false);
        state = UsineAssemblageState.rule;
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.AssemblyGame)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    public void UbdateUI()
    {
        UpdateTexts();
    }

    public void UpdateTimeRemaining(int time)
    {
        _time = time;
        UpdateTexts();
    }

    //fct pour géré la win ou la lose du joueur à la fin d'une partie
    public void PlayerHasLose()
    {
        state = UsineAssemblageState.menu;

        //On affiche les infos
        UpdateTexts();

        PanelLose.SetActive(true);
        AudioManager.Instance.PlaySoundEffet(AudioType.Deffaite);
        PanelNotifyAcceleration.SetActive(false);
    }

    //fct pour géré la win ou la lose du joueur à la fin d'une partie
    public void PlayerHasWin()
    {
        state = UsineAssemblageState.menu;

        //On affiche les infos
        UpdateTexts();

        PanelWin.SetActive(true);
        AudioManager.Instance.PlaySoundEffet(AudioType.Victory);
        PanelNotifyAcceleration.SetActive(false);
    }

    //pour lancer la première partie
    public void RunGame()
    {
        if (state != UsineAssemblageState.rule) return;
        state = UsineAssemblageState.game; //on passe en mode jeux
        //on lance le jeux
        UsineAssemblageGameManager.Instance.RunGame();

        //on s'ocupe de l'affichage qui change
        PanelRuler.SetActive(false);
        PanelInformation.SetActive(true);
    }


    //Fct pour rejouer une game
    public void OnReplayButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
        state = UsineAssemblageState.rule;

        //On affiche les bon Panel
        PanelRuler.SetActive(true);
        PanelInformation.SetActive(false);
        PanelWin.SetActive(false);
        PanelLose.SetActive(false);
        PanelNotifyAcceleration.SetActive(false);


        UsineAssemblageGameManager.Instance.InitialiseGame();
    }

    //Fct pour sortir du mini jeux
    public void OnQuitButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
        Debug.Log("Quit button clicked. Hiding panels.");
        Time.timeScale = 1.0f;
        PanelLose.SetActive(false);
        PanelWin.SetActive(false);
        PanelNotifyAcceleration.SetActive(false);
        PanelRuler.SetActive(false);
        PanelInformation.SetActive(false);
        StartCoroutine(ChargementTransitionManager.Instance.LoadScene(GameProgressManager.GameProgressState.AssemblyZoneEnd, "AssemblageJeux", "UsineAssemblage", true));
    }

    //Getter state
    public UsineAssemblageState State
    {
        get { return state; }
    }

    public void ShowNotifyAcceleration()
    {
        PanelNotifyAcceleration.SetActive(true);
    }
    public void HideNotifyAcceleration()
    {
        PanelNotifyAcceleration.SetActive(false);
    }

    private void UpdateTexts()
    {
        if (txtNbCircuitWin == null || txtTime == null || scoreNumberWin == null || scoreNumberLoose == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;
        }

        if (UsineAssemblageGameManager.Instance == null)
            return;

        txtNbCircuitWin.text = LanguageManager.Instance.GetText("completedCircuit") + " : " + UsineAssemblageGameManager.Instance.GetNbCircuitWin().ToString() + "/" + UsineAssemblageGameManager.Instance.GetNbCircuitGoal().ToString();
        txtTime.text = LanguageManager.Instance.GetText("timeRemaining") + " : " + _time.ToString();

        scoreNumberWin.text = UsineAssemblageGameManager.Instance.GetNbCircuitWin() + " " + LanguageManager.Instance.GetText("completedCircuitLittle");
        scoreNumberLoose.text = UsineAssemblageGameManager.Instance.GetNbCircuitWin() + " " + LanguageManager.Instance.GetText("completedCircuitLittle");

        txtRules.text = LanguageManager.Instance.GetText("rules");
        txtWin.text = LanguageManager.Instance.GetText("win");
        txtLose.text = LanguageManager.Instance.GetText("lose");
        txtReplayLose.text = LanguageManager.Instance.GetText("replay");
        txtQuitLose.text = LanguageManager.Instance.GetText("quit");
        txtReplayWin.text = LanguageManager.Instance.GetText("replay");
        txtQuitWin.text = LanguageManager.Instance.GetText("quit");
        txtNotifyAcceleration.text = LanguageManager.Instance.GetText("notifyAcceleration");
    }
}
