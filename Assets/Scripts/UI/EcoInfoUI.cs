using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EcoInfoUI : Menu
{
    public Animator animator;

    [Header("GameObject")]
    [SerializeField] private GameObject panelEcoInfo;
    [SerializeField] private GameObject ecoInfoPanel;
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
        LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
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
}
