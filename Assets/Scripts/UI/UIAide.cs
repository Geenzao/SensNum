using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIAide : Menu
{
    [Header("Text")]
    public TextMeshProUGUI texteTitreAide;
    public TextMeshProUGUI texteTitrePanelControls;
    public TextMeshProUGUI texteTextEspace;
    public TextMeshProUGUI texteTextDeplacement;
    public TextMeshProUGUI texteTextInteractionE;
    public TextMeshProUGUI texteTextPause;
    public TextMeshProUGUI texteTitrePanelObj;
    public TextMeshProUGUI texteTextObj;

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

        if (texteTitreAide == null || texteTitrePanelControls == null || texteTextEspace == null 
            || texteTextDeplacement == null || texteTextInteractionE == null || texteTextPause == null || texteTextObj == null)
        {
            Debug.LogError("Text elements are not assigned in the inspector.");
            return;

        }
        else
        {
            texteTitreAide.text = LanguageManager.Instance.GetText("AideTitre");
            texteTitrePanelControls.text = LanguageManager.Instance.GetText("AideTitreControl");
            texteTextEspace.text = LanguageManager.Instance.GetText("AideEspaceText");
            texteTextDeplacement.text = LanguageManager.Instance.GetText("AideDeplacementText");
            texteTextInteractionE.text = LanguageManager.Instance.GetText("AideInteractionText");
            texteTextPause.text = LanguageManager.Instance.GetText("AidePauseText");
            texteTitrePanelObj.text = LanguageManager.Instance.GetText("AideTitreObjectif");
            texteTextObj.text = LanguageManager.Instance.GetText("AideObjectifText");
        }
    }
}
