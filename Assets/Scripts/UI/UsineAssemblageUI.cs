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
    public TextMeshProUGUI txtNbCircuitWin;
    public TextMeshProUGUI txtTime;
    //Nouveau

    public TextMeshProUGUI txtInfoGameWin_nbCircuitWin; //Pour afficher le nombre de circuit réalisé pendant la game
    public TextMeshProUGUI txtInfoGameWin_nbCircuitLose; //Pour afficher le nombre de circuit échoué pendant la game
    public TextMeshProUGUI txtInfoGameWin_TimeForWin; //Pour afficher le temps que le joueur a mit pour atteindre l'objectif

    public TextMeshProUGUI txtInfoGameLose_nbCircuitWin; //Pour afficher le nombre de circuit réalisé pendant la game
    public TextMeshProUGUI txtInfoGameLose_nbCircuitLose; //Pour afficher le nombre de circuit échoué pendant la game


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


    private UsineAssemblageState state;

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.AssemblyGame)
        {
            TriggerVisibility(true);
        }

        btnReplayWin.onClick.AddListener(OnReplayButtonClicked);
        btnReplayLose.onClick.AddListener(OnReplayButtonClicked);
        btnQuitWin.onClick.AddListener(OnQuitButtonClicked);
        btnQuitLose.onClick.AddListener(OnQuitButtonClicked);
    }

    protected override void TriggerVisibility(bool visible)
    {
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
        string txt = "Circuit réalisé : " + UsineAssemblageGameManager.Instance.GetNbCircuitWin().ToString() + "/" + UsineAssemblageGameManager.Instance.GetNbCircuitGoal().ToString();
        txtNbCircuitWin.text = txt;
    }

    public void UpdateTimeRemaining(int time)
    {
        string txt = "Temps restant : " + time.ToString();
        txtTime.text = txt;
    }

    //fct pour géré la win ou la lose du joueur à la fin d'une partie
    public void PlayerHasLose()
    {
        state = UsineAssemblageState.menu;

        //On affiche les infos
        txtInfoGameLose_nbCircuitWin.text = UsineAssemblageGameManager.Instance.GetNbCircuitWin() + " circuit réalisé";
        txtInfoGameLose_nbCircuitLose.text = UsineAssemblageGameManager.Instance.GetNbCircuitLose() + " circuit mal fait";

        PanelLose.SetActive(true);
        PanelNotifyAcceleration.SetActive(false);
    }

    //fct pour géré la win ou la lose du joueur à la fin d'une partie
    public void PlayerHasWin()
    {
        state = UsineAssemblageState.menu;

        //On affiche les infos
        txtInfoGameWin_nbCircuitWin.text = UsineAssemblageGameManager.Instance.GetNbCircuitWin() + " circuit réalisé";
        txtInfoGameWin_nbCircuitLose.text = UsineAssemblageGameManager.Instance.GetNbCircuitLose() + " circuit mal fait";
        txtInfoGameWin_TimeForWin.text = "Objectif atteint en : " + UsineAssemblageGameManager.Instance.GetTimeForGoal() + " secondes";

        PanelWin.SetActive(true);
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
        print("Vous voulez Rejouer");
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
        //TODO : a faire
        print("Vous voulez Abandonner");
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
}
