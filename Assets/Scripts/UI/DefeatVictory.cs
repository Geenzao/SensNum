using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DefeatVictory : Menu
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI titleWin;
    [SerializeField] private TextMeshProUGUI titleLoose;
    [SerializeField] private TextMeshProUGUI scroreNumber;

    [Header("Button")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button backSceneButton;

    [Header("Panel")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;

    [Header("Variable")]
    [SerializeField] private int score;
    [SerializeField] private string LastSceneName;

    private void Awake()
    {
        retryButton.onClick.AddListener(OnRetryButtonClicked);
        backSceneButton.onClick.AddListener(OnBackSceneButtonClicked);
    }

    // ----------------- TO DO : RECOMMENCER LE MINI-JEU -----------------\\
    private void OnRetryButtonClicked()
    {
        gameObject.SetActive(false);
    }

    private void OnBackSceneButtonClicked()
    {
        gameObject.SetActive(false);
        GameManager.Instance.LoadLevelAndPositionPlayer(LastSceneName);
        GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.Start);
    }

    private void UpdateTexts()
    {
        titleWin.text = LanguageManager.Instance.GetText("win");
        titleLoose.text = LanguageManager.Instance.GetText("lose");
    }
}
