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
    [SerializeField] private TextMeshProUGUI titleWinText;
    [SerializeField] private TextMeshProUGUI titleLooseText;
    [SerializeField] private TextMeshProUGUI scoreNumberWinText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Button")]
    [SerializeField] private Button winRetryButton;
    [SerializeField] private Button looseRetryButton;
    [SerializeField] private Button winNextGameButton;

    [Header("Panel")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
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
    private SpawnAndDropManager spawnAndDropManager;
    private float _timer;

    private void Awake()
    {
        winRetryButton.onClick.AddListener(OnRetryButtonClicked);
        looseRetryButton.onClick.AddListener(OnRetryButtonClicked);
        winNextGameButton.onClick.AddListener(OnBackSceneButtonClicked);

        LanguageManager.OnLanguageChanged += UpdateTexts;
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
            TriggerVisibility(true);
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
                _timer = timer;
                UpdateTexts();

                if (timer <= 0.0f)
                {
                    timer = 0.0f;
                    _timer = timer;
                    UpdateTexts();
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
            AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
            StartGame();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        gameStarted = true;
        panelTexteDebut.gameObject.SetActive(false);
        if (btnVert != null)
        {
            btnVert.SetActive(false);
        }
        spawnAndDropManager.StartGame();
    }

    private void EndGame()
    {
        if (!isStopped)
        {
            AudioManager.Instance.PlaySoundEffet(AudioType.Victory);
            isStopped = true;
            UpdateTexts();
            Time.timeScale = 0.0f;
            if (oreCounter.cptAu != 0 || oreCounter.cptCu != 0 || oreCounter.cptLi != 0)
                winPanel.gameObject.SetActive(true);
            else
                loosePanel.gameObject.SetActive(true);
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
    }

    private void UpdateUI()
    {
        UpdateTexts();
    }

    private void OnRetryButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
        winPanel.gameObject.SetActive(false);
        loosePanel.gameObject.SetActive(false);
        oreCounter.cptAu = 0;
        oreCounter.cptCu = 0;
        oreCounter.cptLi = 0;
        timer = 30.0f;
        isStopped = false;
        gameStarted = false;
        panelTexteDebut.gameObject.SetActive(true);
        UpdateTexts();
        btnVert.SetActive(true);
    }

    private void OnBackSceneButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
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

        else
        {
            if (oreCounter == null)
            {
                /*Debug.Log("OreCounter is not assigned.");*/
                return;
            }

            else
            {
                titleWinText.text = LanguageManager.Instance.GetText("EndTextWin_MJ1");
                titleLooseText.text = LanguageManager.Instance.GetText("EndTextLose_MJ1");
                texteCptOr.text = LanguageManager.Instance.GetText("gold") + " : " + oreCounter.cptAu.ToString();
                texteCptCu.text = LanguageManager.Instance.GetText("copper") + " : " + oreCounter.cptCu.ToString();
                texteCptLi.text = LanguageManager.Instance.GetText("lithium") + " : " + oreCounter.cptLi.ToString();
                texteTimer.text = LanguageManager.Instance.GetText("chrono") + " : " + Mathf.FloorToInt(_timer).ToString();
                texteDebut.text = LanguageManager.Instance.GetText("begining");
                texteFin.text = LanguageManager.Instance.GetText("end") + "\n" + texteCptOr.text + "\n" + texteCptCu.text + "\n" + texteCptLi.text;
                scoreNumberWinText.text = texteCptOr.text + "\n" + texteCptCu.text + "\n" + texteCptLi.text;
                scoreText.text = LanguageManager.Instance.GetText("score");
            }
        }
            
    }

}
