using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;



/*
 *
 *A voir si on ne fait pas un seul canvas pour tous les minu jeux avec la m�me DA ou un par mini jeux
 *
 *
 *
 */

public enum UsineAssemblageState
{
    rule, //moment o� on explique les regles
    game, //moment ou on joue
    menu //moment o� il y a un menu win ou lose d'afficher
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

    public TextMeshProUGUI txtInfoGameWin_nbCircuitWin; //Pour afficher le nombre de circuit r�alis� pendant la game
    public TextMeshProUGUI txtInfoGameWin_nbCircuitLose; //Pour afficher le nombre de circuit �chou� pendant la game
    public TextMeshProUGUI txtInfoGameWin_TimeForWin; //Pour afficher le temps que le joueur a mit pour atteindre l'objectif

    public TextMeshProUGUI txtInfoGameLose_nbCircuitWin; //Pour afficher le nombre de circuit r�alis� pendant la game
    public TextMeshProUGUI txtInfoGameLose_nbCircuitLose; //Pour afficher le nombre de circuit �chou� pendant la game


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
        LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
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

    //fct pour g�r� la win ou la lose du joueur � la fin d'une partie
    public void PlayerHasLose()
    {
        state = UsineAssemblageState.menu;

        //On affiche les infos
        UpdateTexts();

        PanelLose.SetActive(true);
        PanelNotifyAcceleration.SetActive(false);
    }

    //fct pour g�r� la win ou la lose du joueur � la fin d'une partie
    public void PlayerHasWin()
    {
        state = UsineAssemblageState.menu;

        //On affiche les infos
        UpdateTexts();

        PanelWin.SetActive(true);
        PanelNotifyAcceleration.SetActive(false);
    }

    //pour lancer la premi�re partie
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
        if (txtNbCircuitWin == null || txtTime == null || txtInfoGameWin_nbCircuitWin == null || txtInfoGameWin_nbCircuitLose == null || txtInfoGameWin_TimeForWin == null || txtInfoGameLose_nbCircuitWin == null || txtInfoGameLose_nbCircuitLose == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;
        }
        if (UsineAssemblageGameManager.Instance == null)
            return;
        txtNbCircuitWin.text = LanguageManager.Instance.GetText("completedCircuit") + " : " + UsineAssemblageGameManager.Instance.GetNbCircuitWin().ToString() + "/" + UsineAssemblageGameManager.Instance.GetNbCircuitGoal().ToString();
        txtTime.text = LanguageManager.Instance.GetText("timeRemaining") + " : " + _time.ToString();

        txtInfoGameWin_nbCircuitWin.text = UsineAssemblageGameManager.Instance.GetNbCircuitWin() + " " + LanguageManager.Instance.GetText("completedCircuitLittle");
        txtInfoGameWin_nbCircuitLose.text = UsineAssemblageGameManager.Instance.GetNbCircuitLose() + " " + LanguageManager.Instance.GetText("poorlyMadeCircuit");
        txtInfoGameWin_TimeForWin.text = LanguageManager.Instance.GetText("objectiveAchievedIn") + " : " + UsineAssemblageGameManager.Instance.GetTimeForGoal() + " " + LanguageManager.Instance.GetText("seconds");
        scoreNumberWin.text = txtInfoGameWin_nbCircuitWin.text + "\n" + txtInfoGameWin_nbCircuitLose.text;

        txtInfoGameLose_nbCircuitWin.text = UsineAssemblageGameManager.Instance.GetNbCircuitWin() + " " + LanguageManager.Instance.GetText("completedCircuitLittle");
        txtInfoGameLose_nbCircuitLose.text = UsineAssemblageGameManager.Instance.GetNbCircuitLose() + " " + LanguageManager.Instance.GetText("poorlyMadeCircuit");
        scoreNumberLoose.text = txtInfoGameLose_nbCircuitWin.text + "\n" + txtInfoGameLose_nbCircuitLose.text;

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
