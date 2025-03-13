using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIAide : Menu
{
    [Header("Text")]
    public TextMeshProUGUI TitreAide;
    public TextMeshProUGUI TitrePanelControls;
    public TextMeshProUGUI TextEspace;
    public TextMeshProUGUI TextDeplacement;
    public TextMeshProUGUI TextInteractionE;
    public TextMeshProUGUI TextPause;
    public TextMeshProUGUI TitrePanelObj;
    public TextMeshProUGUI TextObj;

    [Header("Button")]
    [SerializeField] private Button ClosePageAide;
    [SerializeField] private Button OpenPageAide;

    [Header("Panel")]
    [SerializeField] private GameObject PanelAide;
    [SerializeField] private GameObject PanelBtn;


    private void Awake()
    {
        LanguageManager.OnLanguageChanged += UpdateTexts;

        ClosePageAide.onClick.AddListener(OnClosePageAide);
        OpenPageAide.onClick.AddListener(OnOpenPageAide);
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.None)
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
            PanelBtn.SetActive(visible);
        }
        else
        {
            PanelBtn.SetActive(false);
        }
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.SecondGameMine)
            TriggerVisibility(true);
        else
            TriggerVisibility(false);
    }

    public void OnOpenPageAide()
    {
        PanelAide.SetActive(true);
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
    }
    
    public void OnClosePageAide()
    {
        PanelAide.SetActive(false);
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
    }

    private void UpdateTexts()
    {
        //if (texteCptOr == null || texteCptCu == null || texteCptLi == null || texteTimer == null || texteDebut == null || texteFin == null)
        //{
        //    Debug.LogError("Text elements are not assigned in the inspector.");
        //    return;
        //}

        //else
        //{
        //    if (oreCounter == null)
        //    {
        //        Debug.Log("OreCounter is not assigned.");
        //        return;
        //    }

        //    else
        //    {
        //        //titleWinText.text = LanguageManager.Instance.GetText("win");
        //        //texteCptOr.text = LanguageManager.Instance.GetText("gold") + " : " + oreCounter.cptAu.ToString();
        //        //texteCptCu.text = LanguageManager.Instance.GetText("copper") + " : " + oreCounter.cptCu.ToString();
        //        //texteCptLi.text = LanguageManager.Instance.GetText("lithium") + " : " + oreCounter.cptLi.ToString();
        //        //texteTimer.text = LanguageManager.Instance.GetText("chrono") + " : " + Mathf.FloorToInt(_timer).ToString();
        //        //texteDebut.text = LanguageManager.Instance.GetText("begining");
        //        //texteFin.text = LanguageManager.Instance.GetText("end") + "\n" + texteCptOr.text + "\n" + texteCptCu.text + "\n" + texteCptLi.text;
        //        //scoreNumberWinText.text = texteCptOr.text + "\n" + texteCptCu.text + "\n" + texteCptLi.text;
        //        //scoreText.text = LanguageManager.Instance.GetText("score");
        //    }
        //}

    }
}
