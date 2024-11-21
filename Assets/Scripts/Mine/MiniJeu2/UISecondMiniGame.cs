using System.Collections;
using TMPro;
using UnityEngine;

public class UISecondMiniGame : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI texteCptOr;
    public TextMeshProUGUI texteCptCu;
    public TextMeshProUGUI texteCptLi;
    public TextMeshProUGUI texteTimer;
    public TextMeshProUGUI texteDebut;
    public TextMeshProUGUI texteFin;

    public float timer = 30.0f;
    private bool isStopped = false;
    private bool gameStarted = false;

    private DropZone dropZone;

    private void Start()
    {
        dropZone = Object.FindFirstObjectByType<DropZone>();

        if (dropZone == null)
        {
            Debug.LogError("DropZone not found");
            return;
        }

        UpdateUI();
    }

    private void Update()
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
            StartGame();
        }

        UpdateUI();
    }

    private void StartGame()
    {
        Time.timeScale = 1.0f;
        gameStarted = true;
        texteDebut.gameObject.SetActive(false);
    }

    private void EndGame()
    {
        if (!isStopped)
        {
            isStopped = true;
            texteFin.text = "Fin du jeu !\nOr : " + dropZone.CptOr + "\nCuivre : " + dropZone.CptCu + "\nLithium : " + dropZone.CptLi;
            Time.timeScale = 0.0f;
            texteFin.gameObject.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        texteCptOr.text = "Or : " + dropZone.CptOr;
        texteCptCu.text = "Cuivre : " + dropZone.CptCu;
        texteCptLi.text = "Lithium : " + dropZone.CptLi;
    }
}