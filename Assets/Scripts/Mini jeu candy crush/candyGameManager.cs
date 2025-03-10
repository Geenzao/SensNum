using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;


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

    public bool gameassarted = false;

    public bool isGameEnded;

    public float tempsDerniereExecution = 0.0f; // stock le temps passé depuis la derniere execution;
    public float delai = 5.0f;
    int nbcoup;// tu defini l'interval voulu, en seconde.	

    void Update()
    {
        if (gameassarted)
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
        
    }
    void MonAction()
    {
        nbDechets++;
        nbcoup++;
        if (nbDechets < 11)
        {
            barredechet = nbDechets / 10f;
        }
        if(nbcoup >= 3 && delai > 0.5f)
        {
            delai = delai - 0.5f;
            nbcoup = 0;
        }
    }

    void Start()
    {
        nbDechets = 0f;
    }

    public void ProcessTurn(int pointGain,int coupRealise)
    {
        gameassarted = true;
        points += pointGain;
        pointText = points;

        if (coupRealise == 1)
        {
            AudioManager.Instance.PlaySoundEffet(AudioType.CandyCrushMatch);
            nbMatchs++;
            if(nbDechets > 0)
            {
                nbDechets-=1;
            }
            nbMatchsText = nbMatchs;
        }
        if (coupRealise == 2)
        {
            AudioManager.Instance.PlaySoundEffet(AudioType.CandyCrushSuperMatch);
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
