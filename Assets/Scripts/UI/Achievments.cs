using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Achievments : Menu
{
    [Header("Button")]
    [SerializeField] private Button buttonAchievments;

    [Header("Image")]
    [SerializeField] private Image mineAchievment;
    [SerializeField] private Image factoryAchievment;
    [SerializeField] private Image recycleAchievment;

    [Header("Panel")]

    [SerializeField] private GameObject panelButton;
    [SerializeField] private GameObject panelAchievments;

    [Header("Animator")]
    [SerializeField] private Animator animatorPanelArchievments;
    [SerializeField] private bool isActivated = false;
    [SerializeField] private bool isAnimationActive = false; //pour éviter de lancer plusieurs fois l'animation

    private Coroutine blinkCoroutine;
    private int breaker = 0;

    private void Awake()
    {
        buttonAchievments.onClick.AddListener(OnAchievmentsButtonClicked);
    }

    private void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.None)
        {
            TriggerVisibility(true);
        }
    }

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        if (visible)
        {
            panelButton.SetActive(visible);
            panelAchievments.SetActive(visible);
        }
        else
        {
            panelButton.SetActive(visible);
            panelAchievments.SetActive(visible);
        }
    }

    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.None)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
    }

    private void Update()
    {
        switch (GameProgressManager.CurrentGameProgressState)
        {
            case GameProgressManager.GameProgressState.MineEnd:
                if (breaker == 0)
                {
                    breaker = 1;
                    SetAchievment(0);
                }
                break;
            case GameProgressManager.GameProgressState.AssemblyZoneEnd:
                if (breaker == 1)
                {
                    breaker = 2;
                    SetAchievment(1);
                }
                break;
            case GameProgressManager.GameProgressState.RecyclingEnd:
                if (breaker == 2)
                {
                    breaker = 3;
                    SetAchievment(2);
                }
                break;
        }
    }

    private void OnAchievmentsButtonClicked()
    {
        if (isAnimationActive)
            return;
        if (isActivated)
        {
            animatorPanelArchievments.SetTrigger("hide");
            isActivated = false;
        }
        else
        {
            animatorPanelArchievments.SetTrigger("show");
            isActivated = true;
        }
    }

    public void AnimationStart()
    {
        isAnimationActive = true;
    }

    public void AnimationEnd() 
    {
        isAnimationActive = false;
    }

    public void ShowPanel()
    {
        panelAchievments.SetActive(true);
    }

    public void HidePanel() {
        panelAchievments.SetActive(false);
    }

    public void SetAchievment(int index)
    {
        switch (index)
        {
            case 0:
                mineAchievment.color = Color.white;
                break;
            case 1:
                factoryAchievment.color = Color.white;
                break;
            case 2:
                recycleAchievment.color = Color.white;
                break;
        }

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkButton(buttonAchievments, 5f));
    }

    private IEnumerator BlinkButton(Button button, float duration)
    {
        float elapsedTime = 0f;
        Color originalColor = button.image.color;
        Color blinkColor = Color.yellow;
        bool isBlinking = false;

        while (elapsedTime < duration)
        {
            button.image.color = isBlinking ? originalColor : blinkColor;
            isBlinking = !isBlinking;
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        button.image.color = originalColor;
    }
}
