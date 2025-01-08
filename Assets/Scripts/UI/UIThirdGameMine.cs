using Settings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UIThirdGameMine : Menu
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI textDebut;

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
            textDebut.gameObject.SetActive(false);
            countText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            winText.gameObject.SetActive(false);
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
            textDebut.gameObject.SetActive(visible);
            countText.gameObject.SetActive(visible);
            timerText.gameObject.SetActive(visible);
            winText.gameObject.SetActive(false);
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
        textDebut.gameObject.SetActive(false);

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
                winText.gameObject.SetActive(true);
            }
            else
            {
                UpdateTexts();
                loseText.gameObject.SetActive(true);
            }
            Time.timeScale = 0.0f;
        }
    }

    private void UpdateTexts()
    {
        if (countText == null || winText == null || timerText == null || textDebut == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;
        }

        countText.text = LanguageManager.Instance.GetText("truck") + " : " + counterTruck + "/" + maxTruck; ;
        winText.text = LanguageManager.Instance.GetText("win");
        loseText.text = LanguageManager.Instance.GetText("lose");
        timerText.text = LanguageManager.Instance.GetText("chrono") + " : " + Mathf.FloorToInt(timer);
        textDebut.text = LanguageManager.Instance.GetText("startThirdGameMine");
    }
}
