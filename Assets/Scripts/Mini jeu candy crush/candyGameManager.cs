using UnityEngine;
using UnityEngine.UI;


public class CandyGameManager : Singleton<CandyGameManager>
{
    public int nbSuperMatchsText { get; set; }
    public int nbMatchsText { get; set; }
    public int pointText { get; set; }

    public float barredechet { get; set; }

    public float nbDechets { get; set; }
    public int nbSuperMatchs = 0;
    public int nbMatchs = 0;
    public int points;

    public bool isGameEnded;

    public float tempsDerniereExecution = 0.0f; // stock le temps passé depuis la derniere execution;
    float delai = 5.0f;    // tu defini l'interval voulu, en seconde.	

    void Update()
    {
        if (nbDechets < 15)
        {
            tempsDerniereExecution += Time.fixedDeltaTime;  // ajoute a chaque update le temps écoulé depuis le dernier Update		
            if (tempsDerniereExecution > delai)
            {
                MonAction();
                tempsDerniereExecution = 0;
            }

        }
        if (nbDechets == 15)
        {
            //Lancer un event pour dire que le jeu est fini
        }
    }
    void MonAction()
    {
        nbDechets++;
        if (nbDechets < 11)
        {
            barredechet = nbDechets / 10f;
        }
    }

    void Start()
    {
        nbDechets = 0f;
    }

    public void ProcessTurn(int pointGain,int coupRealise)
    {
        points += pointGain;
        pointText = points;

        if (coupRealise == 1)
        {
            nbMatchs++;
            if(nbDechets > 0)
            {
                nbDechets-=1;
            }
            nbMatchsText = nbMatchs;
        }
        if (coupRealise == 2)
        {
            nbSuperMatchs++;
            if (nbDechets > 0)
            {
                nbDechets-=5;
            }
            nbSuperMatchsText = nbSuperMatchs;
        }
        if (nbDechets < 11)
        {
            barredechet = nbDechets / 10f;
        }

    }
}
