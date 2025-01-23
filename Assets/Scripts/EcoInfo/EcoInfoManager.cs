using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


//Cette classe permet de gérer les informations écologiques qui seront affichées au joueur pendant la partie
//Elle permet d'afficher des informations écologiques de manière aléatoire à un intervalle de temps donné
//Elle ne fonctionne que durant le mode village pour l'instant


public class EcoInfoManager : Singleton<EcoInfoManager>
{
    public static event Action OnEcoInfoShow;
    public static event Action OnEcoInfoHide;

    //Tab de phrase EcoInfo a afficher au joueur pendant la parti
    private string[] tabEcoInfo = {
    "Chaque smartphone produit nécessite 44,4 kg de matières premières. Pensez à prolonger leur durée de vie !",
    "Le recyclage des équipements numériques consomme beaucoup d'eau et d'énergie. Réduire nos achats est la clé.",
    "L'extraction minière pour les métaux rares pollue l'eau et détruit des écosystèmes entiers.",
    "Utilisez le mode économie d'énergie pour réduire la consommation de vos appareils électroniques.",
    "Les centres de données consomment près de 2% de l'électricité mondiale. Préférez le stockage local.",
    "Recycler ne suffit pas : seuls 20% des déchets électroniques mondiaux sont recyclés correctement.",
    "Éteignez complètement les appareils que vous n'utilisez pas pour économiser de l'énergie.",
    "Privilégier lachat de matériel reconditionné réduit la demande de nouvelles matières premières.",
    "Partager ses équipements numériques prolonge leur durée de vie et réduit l'impact écologique.",
    "L'envoi d'un email avec pièce jointe de 1 Mo produit environ 19 g de CO2."
    };
    
    private int[] tabIndexShowed; //pour ne pas afficher deux fois la même EcoInfo
    private int indexEcoInfo = 0; //pour stocker l'index de l'EcoInfo actuellement affiché

    //Pour le timer
    private float timeBetweenEcoInfo = 90f; //Temps = 1min30
    private float timer = 0f;
    private float TimerBeforeHide = 10f;
    private float timerHide = 0f;


    private bool isFullMessageDisplayed = false; // Indique si le message complet est affiché
    private string fullMessage; // Stocke le message complet en cours
    private string partialMessage;

    private bool isEcoInfoDisplayed = false;

    void Start()
    {
        //On initialise le tableau à -1
        tabIndexShowed = new int[tabEcoInfo.Length];
        for (int i = 0; i < tabIndexShowed.Length; i++)
        {
            tabIndexShowed[i] = -1;
        }
    }

    void Update()
    {
        if (isEcoInfoDisplayed == false)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenEcoInfo || Input.GetKeyDown(KeyCode.V))
            {
                timer = 0f;
                ShowEcoInfo();
            }
        }

        if(isEcoInfoDisplayed && isFullMessageDisplayed == false)
        {
            timerHide += Time.deltaTime;
            if (timerHide >= TimerBeforeHide)
            {
                /****************************************HIDE ECO INFO/****************************************/
                OnEcoInfoHide?.Invoke();
                timerHide = 0f;
            }
        }
    }

    private void ShowEcoInfo()
    {
        //On vérifie s'il y a encore des phrase à afficher
        if (IsThereStillEcoInfoToShow() == false)
            return;

        //On défini l'indice du prochain message à afficher
        int indexToShow = UnityEngine.Random.Range(0, tabEcoInfo.Length);
        while (tabIndexShowed[indexToShow] != -1)
        {
            indexToShow += 1;
            if (indexToShow >= tabEcoInfo.Length)
            {
                indexToShow = 0;
            }
        }
        indexEcoInfo = indexToShow;

        //On stok les message a afficher
        tabIndexShowed[indexToShow] = 1;
        fullMessage = tabEcoInfo[indexToShow]; // Stocke le message complet
        // Affiche les 25 premiers caractères
        partialMessage = fullMessage.Length > 25 ? fullMessage.Substring(0, 25) + "..." : fullMessage;
        isFullMessageDisplayed = false;


        /****************************************SHOW ECO INFO/****************************************/
        OnEcoInfoShow?.Invoke();
        isEcoInfoDisplayed = true;
    }

    //pour savoir s'il reste encore des phrase à afficher
    public bool IsThereStillEcoInfoToShow()
    {
        for (int i = 0; i < tabIndexShowed.Length; i++)
        {
            if (tabIndexShowed[i] == -1)
            {
                return true;
            }
        }
        return false;
    }


    //fonction pour mettre à jour le param : isFullMessageDisplayed
    public void SetIsFullMessageDisplayed(bool value)
    {
        isFullMessageDisplayed = value;
    }

    //fonction pour reset le timerHide
    public void ResetTimerHide()
    {
        timerHide = 0f;
    }

    //pour donne tout le message
    public string GetFullMessage()
    {
        return tabEcoInfo[indexEcoInfo];
    }

    //pour avoir le message partiel
    public string GetPartialMessage()
    {
        return partialMessage;
    }

    public void EcoInfoIsHide()
    {
        isEcoInfoDisplayed = false;
    }
}
