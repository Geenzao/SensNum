using Settings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using UnityEngine.SceneManagement;


public class UISecondMiniGame : Menu
{
    public float timer = 30.0f;

    [Header("Text")]
    public TextMeshProUGUI texteCptOr;
    public TextMeshProUGUI texteCptCu;
    public TextMeshProUGUI texteCptLi;
    public TextMeshProUGUI texteTimer;
    public TextMeshProUGUI texteDebut;
    public TextMeshProUGUI texteFin;
    [SerializeField] private TextMeshProUGUI titleWin;
    [SerializeField] private TextMeshProUGUI scoreNumberWin;

    [Header("Button")]
    [SerializeField] private Button winRetryButton;
    [SerializeField] private Button winNextGameButton;

    [Header("Panel")]
    [SerializeField] private GameObject winPanel;
    public GameObject panelTexteDebut;
    public GameObject panelTexteFin;
    public GameObject panelTexteMinerai;
    public GameObject panelChrono;

    [Header("Variable")]
    [SerializeField] private int score;
    [SerializeField] private string LastSceneName;
    private bool isStopped = false;
    private bool gameStarted = false;

    private OreCounter oreCounter;
    private GameObject btnVert;
    private SpawnAndDropManager spawnAndDropManager; // Référence au SpawnAndDropManager

    private void Awake()
    {
        winRetryButton.onClick.AddListener(OnRetryButtonClicked);
        winNextGameButton.onClick.AddListener(OnBackSceneButtonClicked);

        LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.SecondGameMine)
        {
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
        if (visible)
        {
            StartCoroutine(StartGameCoroutine(visible));
        }
    }

    IEnumerator StartGameCoroutine(bool visible)
    {
        yield return new WaitForSeconds(0.5f);
        oreCounter = GameObject.Find("ZonesRecoltes").GetComponent<OreCounter>();
        btnVert = GameObject.Find("BtnVert");
        spawnAndDropManager = GameObject.Find("SpawnZone").GetComponent<SpawnAndDropManager>();
        panelChrono.SetActive(visible);
        panelTexteMinerai.SetActive(visible);
        panelTexteDebut.SetActive(visible);
        panelTexteFin.SetActive(false);
        UpdateUI();
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.SecondGameMine)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    private void Update()
    {
        if(GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.SecondGameMine)
        {
            if (gameStarted && !isStopped)
            {
                timer -= Time.deltaTime;
                texteTimer.text = "Chrono : " + Mathf.FloorToInt(timer);

                if (timer <= 0.0f)
                {
                    timer = 0.0f;
                    texteTimer.text = "Chrono : " + Mathf.FloorToInt(timer);
                    EndGame();
                }
            }

            if (Input.GetMouseButtonDown(0) && !gameStarted)
            {
                CheckForStartButtonClick();
            }
            if(oreCounter != null)
                UpdateUI();
        }
    }

    private void CheckForStartButtonClick()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == btnVert)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        gameStarted = true;
        /*texteDebut.gameObject.SetActive(false);*/
        panelTexteDebut.gameObject.SetActive(false);
        if (btnVert != null)
        {
            btnVert.SetActive(false); // Cache le bouton après le démarrage du jeu
        }
        spawnAndDropManager.StartGame(); // Notifie le SpawnAndDropManager de démarrer le jeu
    }

    private void EndGame()
    {
        if (!isStopped)
        {
            isStopped = true;
            texteFin.text = "Fin du jeu !\nOr : " + oreCounter.cptAu.ToString() + "\nCuivre : " + oreCounter.cptCu.ToString() + "\nLithium : " + oreCounter.cptLi.ToString();
            Time.timeScale = 0.0f;
            /*texteFin.gameObject.SetActive(true);*/
            /*panelTexteFin.gameObject.SetActive(true);*/
            UpdateTexts();
            winPanel.gameObject.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        texteCptOr.text = oreCounter.cptAu.ToString();
        texteCptCu.text = oreCounter.cptCu.ToString();
        texteCptLi.text = oreCounter.cptLi.ToString();
    }

    // ----------------- TO DO : RECOMMENCER LE MINI-JEU -----------------\\
    private void OnRetryButtonClicked()
    {
        winPanel.gameObject.SetActive(false);
        texteCptCu.text = "0";
        texteCptLi.text = "0";
        texteCptOr.text = "0";
        oreCounter.cptAu = 0;
        oreCounter.cptCu = 0;
        oreCounter.cptLi = 0;
        timer = 30.0f;
        isStopped = false;
        gameStarted = false;
        panelTexteDebut.gameObject.SetActive(true);
        UpdateTexts();
        btnVert.SetActive(true);
        //Faire en sorte de supprimer tous les objets de tags "Gold", "Copper", "Lithium" & "Rocks"
        GameObject[] objectsG = GameObject.FindGameObjectsWithTag("Gold");
        GameObject[] objectsC = GameObject.FindGameObjectsWithTag("Copper");
        GameObject[] objectsL = GameObject.FindGameObjectsWithTag("Lithium");
        GameObject[] objectsR = GameObject.FindGameObjectsWithTag("Rocks");
        foreach (GameObject obj in objectsG)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in objectsC)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in objectsL)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in objectsR)
        {
            Destroy(obj);
        }

    }

    private void OnBackSceneButtonClicked()
    {
        winPanel.gameObject.SetActive(false);
        GameManager.Instance.UnloadLevel("Mine2emeJeux");
        GameManager.Instance.LoadLevel(LastSceneName);
        GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.ThirdGameMine);
    }

    private void UpdateTexts()
    {
        if (texteCptOr == null || texteCptCu == null || texteCptLi == null || texteTimer == null || texteDebut == null || texteFin == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;
        }
        scoreNumberWin.text = texteCptOr.text + " Or\n" + texteCptCu.text + " Cuivre\n" + texteCptLi.text + " Lithium";
        //texteCptOr.text = LanguageManager.Instance.GetText("Or");
        //texteCptCu.text = LanguageManager.Instance.GetText("Cuivre");
        //texteCptLi.text = LanguageManager.Instance.GetText("Lithium");
        //texteTimer.text = LanguageManager.Instance.GetText("Chrono");
        //texteDebut.text = LanguageManager.Instance.GetText("Debut");
        //texteFin.text = LanguageManager.Instance.GetText("Fin");
        titleWin.text = LanguageManager.Instance.GetText("win");
    }
}
