using Settings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using UnityEngine.SceneManagement;

public class UIThirdGameMine : Menu
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI textDebut;
    /* ---------- Ajouts Aymeric Debut ---------- */
    [SerializeField] private TextMeshProUGUI titleWin;
    [SerializeField] private TextMeshProUGUI titleLoose;
    [SerializeField] private TextMeshProUGUI scoreNumberLoose;
    [SerializeField] private TextMeshProUGUI scoreNumberWin;

    [Header("Button")]
    [SerializeField] private Button looseRetryButton;
    [SerializeField] private Button looseBackSceneButton;
    [SerializeField] private Button winRetryButton;
    [SerializeField] private Button winBackSceneButton;

    [Header("Panel")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private GameObject PanelCompteur;
    [SerializeField] private GameObject PanelTimer;
    [SerializeField] private GameObject PanelTextDebut;


    [Header("Variable")]
    [SerializeField] private int score;
    [SerializeField] private string LastSceneName;

    /* ---------- Ajout Aymeric Fin ---------- */

    // Variables pour les compteurs et les �tats du jeu
    public float timer = 30.0f;
    private int counterTruck = 0;
    private int counterTruckOre = 0;
    private int maxTruck = 25;
    private int maxTruckOre = 3;
    private bool isStopped = false;
    private bool gameStarted = false;

    private void Awake()
    {
        /* ---------- Ajouts Aymeric Debut ---------- */
        looseRetryButton.onClick.AddListener(OnRetryButtonClicked);
        looseBackSceneButton.onClick.AddListener(OnBackSceneButtonClicked);
        winRetryButton.onClick.AddListener(OnRetryButtonClicked);
        winBackSceneButton.onClick.AddListener(OnBackSceneButtonClicked);
        /* ---------- Ajout Aymeric Fin ---------- */

        LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.ThirdGameMine)
        {
            Debug.Log("ThirdGameMine");
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
    }

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        //Start couroutine de 2 seconde
        if (visible)
        {
            StartCoroutine(StartGameCoroutine(visible));
        }
        else
        {
            PanelCompteur.SetActive(false);
            PanelTextDebut.SetActive(false);
            PanelTimer.SetActive(false);
            winPanel.SetActive(false);
            loosePanel.SetActive(false);
        }
    }

    IEnumerator StartGameCoroutine(bool visible)
    {
        yield return new WaitForSeconds(0.5f);
        if (ThirdMiniGame.Instance == null)
        {
            Debug.LogError("ThirdMiniGame not found");
        }
        else
        {
            ThirdMiniGame.Instance.ThirdMiniGameFinish += EndGame;

            counterTruck = ThirdMiniGame.Instance.CounterTruck;
            counterTruckOre = ThirdMiniGame.Instance.CounterTruckOre;
            // Initialisation des textes UI
            UpdateTexts();
            PanelCompteur.gameObject.SetActive(visible);
            PanelTextDebut.gameObject.SetActive(visible);
            PanelTimer.gameObject.SetActive(visible);
        }
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.ThirdGameMine)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    void Update()
    {
        if (ThirdMiniGame.Instance)
        { 
            counterTruck = ThirdMiniGame.Instance.CounterTruck;
            counterTruckOre = ThirdMiniGame.Instance.CounterTruckOre;
            //Debug.Log(timer);
            // Si le jeu a commenc� et n'est pas arr�t�
            if (gameStarted && !isStopped)
            {
                timer -= Time.deltaTime;
                UpdateTexts();

                // V�rifier si le timer est �coul�
                if (timer <= 0.0f)
                {
                    Time.timeScale = 0.0f;
                    timer = 0.0f;
                    UpdateTexts();
                    EndGame();
                }
            }

            // D�marrer le jeu au clic de la souris si le jeu n'a pas encore commenc�
            if (Input.GetMouseButtonDown(0) && !gameStarted)
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        Time.timeScale = 1.0f;
        gameStarted = true;
        PanelTextDebut.gameObject.SetActive(false);

    }

    // M�thode pour terminer le jeu
    void EndGame()
    {
        if (!isStopped)
        {
            isStopped = true;
            counterTruck = ThirdMiniGame.Instance.CounterTruck;
            counterTruckOre = ThirdMiniGame.Instance.CounterTruckOre;
            // Afficher le message de fin de jeu en fonction du nombre de camions de minerai
            if (counterTruckOre >= maxTruckOre)
            {
                UpdateTexts();
                winPanel.SetActive(true);
            }
            else
            {
                UpdateTexts();
                loosePanel.SetActive(true);
            }
            Time.timeScale = 0.0f;
        }
    }
    /* ---------- Ajouts Aymeric Debut ---------- */
    // ----------------- TO DO : RECOMMENCER LE MINI-JEU -----------------\\
    private void OnRetryButtonClicked()
    {
        loosePanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        ThirdMiniGame.Instance.counterTruck = 0;
        ThirdMiniGame.Instance.counterTruckOre = 0;
        timer = 30.0f;
        isStopped = false;
        textDebut.gameObject.SetActive(true);
        gameStarted = false;
        UpdateTexts();
        // Placer tous les camions à la position des games object dans le tableau truckposition dans thirdminigame.cs
        for (int i = 0; i < ThirdMiniGame.Instance.truckPosition.Length; i++)
        {
            ThirdMiniGame.Instance.truckPosition[i].GetComponent<MoveTruck>().ResetTruck();
        }

    }


    private void OnBackSceneButtonClicked()
    {
        Time.timeScale = 1.0f;
        loosePanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        //Lancer l'ui ici
        StartCoroutine(ChargementTransitionManager.Instance.LoadScene(GameProgressManager.GameProgressState.MineEnd, "Mine3emeJeux", "Mine", true));
    }
    /* ---------- Ajout Aymeric Fin ---------- */

    private void UpdateTexts()
    {
        if (countText == null || timerText == null || textDebut == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;
        }

        countText.text = LanguageManager.Instance.GetText("truck") + " : " + counterTruck + "/" + maxTruck;
        scoreNumberLoose.text = "Total : " + counterTruck + "/" + maxTruck + "\nMinerais : " + counterTruckOre + "/3";
        scoreNumberWin.text = "Total : " + counterTruck + "/" + maxTruck + "\nMinerais : " + counterTruckOre + "/3";
        timerText.text = LanguageManager.Instance.GetText("chrono") + " : " + Mathf.FloorToInt(timer);
        textDebut.text = LanguageManager.Instance.GetText("startThirdGameMine");
        /* ---------- Ajouts Aymeric Debut ---------- */
        titleWin.text = LanguageManager.Instance.GetText("win");
        titleLoose.text = LanguageManager.Instance.GetText("lose");
        /* ---------- Ajout Aymeric Fin ---------- */
    }
}
