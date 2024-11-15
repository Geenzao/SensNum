using TMPro;
using UnityEngine;



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

public class UsineAssemblageUIManager : Singleton<UsineAssemblageUIManager>
{

    //Pour l'UI
    public TextMeshProUGUI txtNbCircuitWin;
    public TextMeshProUGUI txtTime;

    public GameObject PanelRuler;
    public GameObject PanelInformation;
    public GameObject PanelWin;
    public GameObject PanelLose;

    private UsineAssemblageState state;

    void Start()
    {
        PanelRuler.SetActive(true);
        PanelInformation.SetActive(false);
        PanelWin.SetActive(false);
        PanelLose.SetActive(false);
        state = UsineAssemblageState.rule;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UsineAssemblageUIManager.Instance.RunGame();
        }
    }


    public void UbdateUI()
    {
        string txt = "Circuit réalisé : "+ UsineAssemblageGameManager.Instance.GetNbCircuitWin().ToString() + "/" + UsineAssemblageGameManager.Instance.GetNbCircuitGoal().ToString();
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
        PanelLose.SetActive(true);
    }

    //fct pour géré la win ou la lose du joueur à la fin d'une partie
    public void PlayerHasWin()
    {
        state = UsineAssemblageState.menu;
        PanelWin.SetActive(true);
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
    public void Replay()
    {
        print("Vous voulez Rejouer");
        state = UsineAssemblageState.rule;

        //On affiche les bon Panel
        PanelRuler.SetActive(true);
        PanelInformation.SetActive(false);
        PanelWin.SetActive(false);
        PanelLose.SetActive(false);

        UsineAssemblageGameManager.Instance.InitialiseGame();
    }

    //Fct pour sortir du mini jeux
    public void Abandon()
    {
        //TODO : a faire
        print("Vous voulez Abandonner");
    }
}
