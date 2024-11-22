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

    public GameObject btnVert;
    public GameObject panelTexteDebut;
    public GameObject panelTexteFin;
    public SpawnAndDropManager spawnAndDropManager; // Référence au SpawnAndDropManager

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
            CheckForStartButtonClick();
        }

        UpdateUI();
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
            texteFin.text = "Fin du jeu !\nOr : " + dropZone.CptOr + "\nCuivre : " + dropZone.CptCu + "\nLithium : " + dropZone.CptLi;
            Time.timeScale = 0.0f;
            /*texteFin.gameObject.SetActive(true);*/
            panelTexteFin.gameObject.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        texteCptOr.text = "Or : " + dropZone.CptOr;
        texteCptCu.text = "Cuivre : " + dropZone.CptCu;
        texteCptLi.text = "Lithium : " + dropZone.CptLi;
    }
}
