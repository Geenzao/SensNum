using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EcoInfoUI : Menu
{
    public Animator animator;

    [Header("GameObject")]
    [SerializeField] private GameObject panelEcoInfo;
    [SerializeField] private GameObject PanelAllMessage;

    [Header("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI txtEcoInfo;
    [SerializeField] private TextMeshProUGUI txtEcoInfoFullMessage;

    [Header("Button")]
    [SerializeField] private Button btnShowEcoInfoComplet; //pour afficher le message complet
    [SerializeField] private Button btnHideEcoInfo; //pour cacher le message
    [SerializeField] private Button btnHideAllMessage; //pour cacher le grand message


    private void Awake()
    {
        LanguageManager.OnLanguageChanged += UpdateTexts;

        EcoInfoManager.OnEcoInfoShow += ShowEcoInfo;
        EcoInfoManager.OnEcoInfoHide += HideEcoInfo;
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

        PanelAllMessage.SetActive(false);

        btnShowEcoInfoComplet.onClick.AddListener(ShowAllMessage);
        btnHideEcoInfo.onClick.AddListener(HideEcoInfo);
        btnHideAllMessage.onClick.AddListener(HideAllMessage);
    }

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        panelEcoInfo.SetActive(visible);
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.None)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    private void UpdateTexts()
    {
        //if ()
        //{
        //    Debug.LogError("Text elements are not assigned in the inspector.");
        //    return;
        //}
    }

    /**************************************************************************************/
    public void ShowAllMessage()
    {
        txtEcoInfoFullMessage.text = EcoInfoManager.Instance.GetFullMessage();
        PanelAllMessage.SetActive(true);
        EcoInfoManager.Instance.SetIsFullMessageDisplayed(true);
    }

    public void HideAllMessage()
    {
        PanelAllMessage.SetActive(false);
        EcoInfoManager.Instance.SetIsFullMessageDisplayed(false);
    }
    public void HideEcoInfo()
    {
        animator.SetTrigger("HideEcoInfo");
        EcoInfoManager.Instance.ResetTimerHide(); //dans tous les cas
        EcoInfoManager.Instance.SetIsFullMessageDisplayed(false);
        EcoInfoManager.Instance.EcoInfoIsHide();
    }

    public void ShowEcoInfo()
    {
        txtEcoInfo.text = EcoInfoManager.Instance.GetPartialMessage();
        animator.SetTrigger("ShowEcoInfo");
    }
}
