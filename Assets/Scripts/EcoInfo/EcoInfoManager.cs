using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using static PathManager;


//Cette classe permet de gérer les informations écologiques qui seront affichées au joueur pendant la partie
//Elle permet d'afficher des informations écologiques de manière aléatoire à un intervalle de temps donné
//Elle ne fonctionne que durant le mode village pour l'instant


public class EcoInfoManager : Singleton<EcoInfoManager>
{
    public static event Action OnEcoInfoShow;
    public static event Action OnEcoInfoHide;

    //Tab de phrase EcoInfo a afficher au joueur pendant la parti
    public List<List<string>> lstEcoInfo;
    
    private List<List<int>> lstIndexShowed; //pour ne pas afficher deux fois la même EcoInfo
    private int indexEcoInfo = 0; //pour stocker l'index de l'EcoInfo actuellement affiché

    private List<string> lstPhraseIndex = new List<string>();


    //Pour les possibilité on a 
    //0 = village
    //1 = mine
    //2 = assembly
    //3 = recyclage
    //-1 = rien
    private int currentategoryPhraseIndex = 0;

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
        InitializeEcoInfo();

        //On initialise le lstIndexShowed à -1
        lstIndexShowed = new List<List<int>>();
        for (int a = 0; a < lstEcoInfo.Count; a++)
        {
            lstIndexShowed.Add(new List<int>());
            for (int j = 0; j < lstEcoInfo[a].Count; j++)
            {
                lstIndexShowed[a].Add(-1);
            }
        }
    }

    void Update()
    {
        if(UIManager.CurrentMenuState != UIManager.MenuState.None)
        {
            return;
        }

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

    public void InitializeEcoInfo()
    {
        lstPhraseIndex.Add("village");
        lstPhraseIndex.Add("mine");
        lstPhraseIndex.Add("assembly");
        lstPhraseIndex.Add("recyclage");

        currentategoryPhraseIndex = 0;

        //on vide le tableau d'éco in fo
        lstEcoInfo = new List<List<string>>();

        for(int y =0; y < lstPhraseIndex.Count; y++)
        {
            lstEcoInfo.Add(new List<string>());
            int k = 0;
            while (true)
            {
                string key = $"EcoInfo_{lstPhraseIndex[y]}_{k}";
                string text = LanguageManager.Instance.GetText(key);
                if (text == key) // Si le texte retourné est la clé, cela signifie qu'il n'y a plus de texte pour cette catégorie
                {
                    break;
                }
                lstEcoInfo[y].Add(text);
                k++;
            }
        }


    }

    private void ShowEcoInfo()
    {
        //On vérifie s'il y a encore des phrase à afficher
        if (IsThereStillEcoInfoToShow() == false)
            return;

        InitializeEcoInfo();

        //On défini l'indice du prochain message à afficher
        int indexToShow = UnityEngine.Random.Range(0, lstEcoInfo[currentategoryPhraseIndex].Count);
        while (lstIndexShowed[currentategoryPhraseIndex][indexToShow] != -1)
        {
            indexToShow += 1;
            if (indexToShow >= lstEcoInfo[currentategoryPhraseIndex].Count)
            {
                indexToShow = 0;
            }
        }
        indexEcoInfo = indexToShow;

        //On stok les message a afficher
        lstIndexShowed[currentategoryPhraseIndex][indexToShow] = 1;
        fullMessage = lstEcoInfo[currentategoryPhraseIndex][indexToShow]; // Stocke le message complet
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
        currentategoryPhraseIndex = GetcurrentategoryPhraseIndex();
        for (int i = 0; i < lstIndexShowed[currentategoryPhraseIndex].Count; i++)
        {
            if (lstIndexShowed[currentategoryPhraseIndex][i] == -1)
            {
                return true;
            }
        }
        return false;
    }

    public int GetcurrentategoryPhraseIndex()
    {

        if(PathManager.CurrentPathState == PathState.Village1 ||
           PathManager.CurrentPathState == PathState.Village2 ||
           PathManager.CurrentPathState == PathState.Village3)
        {
            return 0;
        }
        else if (PathManager.CurrentPathState == PathState.Mine)
        {
            return 1;
        }
        else if (PathManager.CurrentPathState == PathState.AssemblyFactory)
        {
            return 2;
        }
        else if (PathManager.CurrentPathState == PathState.RecycleFactory)
        {
            return 3;
        }
        else
            return -1;
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
        return lstEcoInfo[currentategoryPhraseIndex][indexEcoInfo];
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
