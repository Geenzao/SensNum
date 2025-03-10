using Settings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UICandyCrush : Menu
{
    [Header("Panel")]
    [SerializeField] private GameObject gamePanel;
    /*[SerializeField] private GameObject backgroundPanel;
    [SerializeField] private GameObject victoryPanel;*/
    [SerializeField] private GameObject defeatPanel;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI nbMatchText;
    [SerializeField] private TextMeshProUGUI nbSuperMatchText;

    [SerializeField] private TextMeshProUGUI scoreIntText;
    [SerializeField] private TextMeshProUGUI nbMatchIntText;
    [SerializeField] private TextMeshProUGUI nbSuperMatchIntText;
    [SerializeField] private TextMeshProUGUI trashToRecycleText;

    [SerializeField] private TextMeshProUGUI titleLoose;
    [SerializeField] private TextMeshProUGUI scoreNumberLoose;

    [SerializeField] private TextMeshProUGUI retryText;
    [SerializeField] private TextMeshProUGUI backSceneButton;

    [Header("Image")]
    [SerializeField] private Image barredechet;

    [Header("GameObjects")]
    [SerializeField] private GameObject alerte;

    [Header("Button")]
    [SerializeField] private Button looseRetryButton;
    [SerializeField] private Button looseBackSceneButton;

    [Header("Variable")]
    [SerializeField] private string LastSceneName;

    private bool isAlreadyFinished = false;

    private void Awake()
    {
        looseRetryButton.onClick.AddListener(OnRetryButtonClicked);
        looseBackSceneButton.onClick.AddListener(OnBackSceneButtonClicked);

        LanguageManager.OnLanguageChanged += UpdateTexts;
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.CandyCrush)
        {
            Debug.Log("CandyCrush");
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
            gamePanel.SetActive(false);
            defeatPanel.SetActive(false);
        }
    }

    IEnumerator StartGameCoroutine(bool visible)
    {
        yield return new WaitForSeconds(0.5f);
        if (CandyGameManager.Instance == null)
        {
            Debug.LogError("Candy crush not found");
        }
        else
        {
            UpdateTexts();
            gamePanel.SetActive(visible);
            defeatPanel.SetActive(false);
        }
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.CandyCrush)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    void Update()
    {
        if (CandyGameManager.Instance)
        {
            barredechet.fillAmount = CandyGameManager.Instance.barredechet;
            scoreIntText.text = CandyGameManager.Instance.pointText.ToString();
            nbMatchIntText.text = CandyGameManager.Instance.nbMatchsText.ToString();
            nbSuperMatchIntText.text = CandyGameManager.Instance.nbSuperMatchsText.ToString();
            if (CandyGameManager.Instance.nbDechets >= 15)
            {
                UpdateTexts();
                if (!isAlreadyFinished)
                {
                    isAlreadyFinished = true;
                    defeatPanel.SetActive(true);
                    AudioManager.Instance.PlaySoundEffet(AudioType.Deffaite);
                    PuceBoard.Instance.isGameFinish = true; //pour empéché le joueur de jouer après la fin du jeu
                }
            }
            if (CandyGameManager.Instance.nbDechets < 10)
                alerte.SetActive(false);
            if (CandyGameManager.Instance.nbDechets >= 10)
                alerte.SetActive(true);
        }
    }

    /* ---------- Ajouts Aymeric Debut ---------- */
    // ----------------- TO DO : RECOMMENCER LE MINI-JEU -----------------\\
    private void OnRetryButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
        /*PuceBoard.Instance.InitializeBoard();*/

        defeatPanel.gameObject.SetActive(false);

        CandyGameManager.Instance.delai = 5.0f;
        CandyGameManager.Instance.gameassarted = false;
        CandyGameManager.Instance.nbDechets = 0;
        CandyGameManager.Instance.points = 0;
        CandyGameManager.Instance.pointText = 0;
        CandyGameManager.Instance.nbMatchs = 0;
        CandyGameManager.Instance.nbMatchsText = 0;
        CandyGameManager.Instance.nbSuperMatchs = 0;
        CandyGameManager.Instance.nbSuperMatchsText = 0;
        CandyGameManager.Instance.barredechet = 0;
        CandyGameManager.Instance.tempsDerniereExecution = 0.0f;

        PuceBoard.Instance.isGameFinish = false; //pour permettre le joueur de jouer 

        UpdateTexts();
        isAlreadyFinished = false;
    }

    // ----------------- TO DO : RETOURNER A LA SCENE PRECEDENTE AVEC BON GAME PROGRESS-----------------\\
    private void OnBackSceneButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
        defeatPanel.gameObject.SetActive(false);
        //GameManager.Instance.UnloadLevel("MiniJeuCandycrush");
        //GameManager.Instance.LoadLevelAndPositionPlayer(LastSceneName);
        //GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.Mine);

        StartCoroutine(ChargementTransitionManager.Instance.LoadScene(GameProgressManager.GameProgressState.RecyclingEnd, "MiniJeuCandycrush", LastSceneName, true));

    }
    /* ---------- Ajout Aymeric Fin ---------- */

    private void UpdateTexts()
    {
        if (scoreText == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;
        }

        scoreText.text = LanguageManager.Instance.GetText("score");
        nbMatchText.text = LanguageManager.Instance.GetText("nbMatch");
        nbSuperMatchText.text = LanguageManager.Instance.GetText("nbSuperMatch");

        trashToRecycleText.text = LanguageManager.Instance.GetText("trashToRecycle");

        titleLoose.text = LanguageManager.Instance.GetText("lose");
        scoreNumberLoose.text = scoreIntText.text;

        retryText.text = LanguageManager.Instance.GetText("replay");
        backSceneButton.text = LanguageManager.Instance.GetText("quit");
    }
}
