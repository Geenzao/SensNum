using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;


//Cette classe permet de gérer les informations écologiques qui seront affichées au joueur pendant la partie
//Elle permet d'afficher des informations écologiques de manière aléatoire à un intervalle de temps donné
//Elle ne fonctionne que durant le mode village pour l'instant


public class EcoInfoManager : MonoBehaviour
{
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
    private int indexEcoInfo = 0;

    //Pour l'UI
    public TextMeshProUGUI txtEcoInfo;
    public TextMeshProUGUI txtEcoInfoFullMessage;
    public GameObject ecoInfoPanel;
    public GameObject PanelAllMessage;

    //Pour le timer
    private float timeBetweenEcoInfo = 10f; //Temps = 1min30
    private float timer = 0f;
    private float TimerBeforeHide = 10f;
    private float timerHide = 0f;

    private Coroutine currentCoroutine = null; // Référence à la coroutine actuelle

    public Animator animator;

    private bool isFullMessageDisplayed = false; // Indique si le message complet est affiché
    private string fullMessage; // Stocke le message complet en cours

    public Button btnShowEcoInfoComplet; //pour afficher le message complet
    public Button btnHideEcoInfo; //pour cacher le message
    public Button btnHideAllMessage; //pour cacher le grand message

    private bool isEcoInfoDisplayed = false;

    void Start()
    {
        //On initialise le tableau à -1
        tabIndexShowed = new int[tabEcoInfo.Length];
        for (int i = 0; i < tabIndexShowed.Length; i++)
        {
            tabIndexShowed[i] = -1;
        }
        btnShowEcoInfoComplet.onClick.AddListener(ShowAllMessage);
        btnHideEcoInfo.onClick.AddListener(HideEcoInfo);
        btnHideAllMessage.onClick.AddListener(HideAllMessage);

        //De base ce message est caché
        PanelAllMessage.SetActive(false);
    }

    void Update()
    {
        print(timer);
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
                HideEcoInfo();
                timerHide = 0f;
            }
        }
    }

    private void ShowEcoInfo()
    {
        //On vérifie s'il y a encore des phrase à afficher
        if(IsThereStillEcoInfoToShow() == false)
            return;

        //On défini l'indice du prochain message à afficher
        int indexToShow = Random.Range(0, tabEcoInfo.Length);
        while (tabIndexShowed[indexToShow] != -1)
        {
            indexToShow += 1;
            if (indexToShow >= tabEcoInfo.Length)
            {
                indexToShow = 0;
            }
        }
        indexEcoInfo = indexToShow;

        //On affiche le message
        tabIndexShowed[indexToShow] = 1;
        fullMessage = tabEcoInfo[indexToShow]; // Stocke le message complet
        // Affiche les 25 premiers caractères
        string partialMessage = fullMessage.Length > 25 ? fullMessage.Substring(0, 25) + "..." : fullMessage;
        txtEcoInfo.text = partialMessage;
        isFullMessageDisplayed = false;

        //txtEcoInfo.text = tabEcoInfo[indexToShow];
        animator.SetTrigger("ShowEcoInfo");
        isEcoInfoDisplayed = true;

        //empecher affichage de plusieur phrase si appuy sur suivant avaant la fin de la premiere coroutine
        //if (currentCoroutine != null)
        //{
        //    StopCoroutine(currentCoroutine);
        //}
        //On lance une coroutine pour cacher le message après un certain temps
        //currentCoroutine = StartCoroutine(HideEcoInfoAfterTime());
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

    public void HideEcoInfo()
    {
        animator.SetTrigger("HideEcoInfo");
        timerHide = 0f; //dans tous les cas
        isEcoInfoDisplayed = false;
    }

    public void ShowEcoInfoManually()
    {
        ShowEcoInfo();
    }

    //IEnumerator HideEcoInfoAfterTime()
    //{
    //    yield return new WaitForSeconds(5f);
    //    HideEcoInfo();
    //}

    public void ShowAllMessage()
    {
        txtEcoInfoFullMessage.text = tabEcoInfo[indexEcoInfo];
        PanelAllMessage.SetActive(true);
        isFullMessageDisplayed = true;
    }

    public void HideAllMessage()
    {
        PanelAllMessage.SetActive(false);
        isFullMessageDisplayed = false;
    }
}
