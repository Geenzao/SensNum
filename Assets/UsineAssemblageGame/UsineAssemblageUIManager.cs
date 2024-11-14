using TMPro;
using UnityEngine;



/*
 *
 *A voir si on ne fait pas un seul canvas pour tous les minu jeux avec la même DA ou un par mini jeux
 *
 *
 *
 */

public class UsineAssemblageUIManager : Singleton<UsineAssemblageUIManager>
{

    //Pour l'UI
    public TextMeshProUGUI txtNbCircuitWin;
    public TextMeshProUGUI txtNbCircuitLose;
    public TextMeshProUGUI txtNbCircuitGoal;

    public GameObject PanelRuler;
    public GameObject PanelInformation;
    public GameObject PanelWin;
    public GameObject PanelLose;

    void Start()
    {
        PanelRuler.SetActive(true);
        PanelInformation.SetActive(false);
        PanelWin.SetActive(false);
        PanelLose.SetActive(false);
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
        txtNbCircuitWin.text = UsineAssemblageGameManager.Instance.GetNbCircuitWin().ToString();
        txtNbCircuitLose.text = UsineAssemblageGameManager.Instance.GetNbCircuitLose().ToString();
    }

    public void RunGame()
    {
        UsineAssemblageGameManager.Instance.RunGame();
        PanelRuler.SetActive(false);
        PanelInformation.SetActive(true);
    }

    public void PlayerHasLose()
    {
        PanelLose.SetActive(true);
    }

    public void PlayerHasWin()
    {
        PanelWin.SetActive(true);
    }
}
