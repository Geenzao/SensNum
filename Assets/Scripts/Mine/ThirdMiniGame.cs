using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ThirdMiniGame : MonoBehaviour
{
    // Variables publiques pour les textes UI et le timer
    public float timer = 30.0f;
    public TMP_Text countText;
    public TMP_Text winText;
    public TMP_Text timerText;
    public TMP_Text textDebut;

    // Variables priv�es pour les compteurs et les �tats du jeu
    private int counterTruck = 0;
    private int counterTruckOre = 0;
    private int maxTruck = 25;
    private int maxTruckOre = 3;
    private bool isStopped = false;
    private bool gameStarted = false;

    void Start()
    {
        // Initialisation des textes UI
        countText.text = "Camions : " + counterTruck + "/" + maxTruck;
        timerText.text = "Chrono : " + Mathf.FloorToInt(timer);
        textDebut.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Si le jeu a commenc� et n'est pas arr�t�
        if (gameStarted && !isStopped)
        {
            // D�cr�menter le timer et mettre � jour les textes UI
            timer -= Time.deltaTime;
            timerText.text = "Chrono : " + Mathf.FloorToInt(timer);
            countText.text = "Camions : " + counterTruck + "/" + maxTruck;

            // V�rifier si le timer est �coul�
            if (timer <= 0.0f)
            {
                EndGame();
            }
        }

        // D�marrer le jeu au clic de la souris si le jeu n'a pas encore commenc�
        if (Input.GetMouseButtonDown(0) && !gameStarted)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        textDebut.gameObject.SetActive(false);
    }

    // M�thode pour terminer le jeu
    void EndGame()
    {
        if (!isStopped)
        {
            isStopped = true;
            // Afficher le message de fin de jeu en fonction du nombre de camions de minerai
            if (counterTruckOre >= maxTruckOre)
            {
                countText.text = "Camions : " + counterTruck + "/" + maxTruck;
                winText.text = "Bravo !";
            }
            else
            {
                winText.text = "T nul PD !";
            }
            winText.gameObject.SetActive(true);
        }
    }

    // M�thode pour incr�menter le compteur de camions
    public void IncrementTruckCounter()
    {
        counterTruck++;
    }

    // M�thode pour incr�menter le compteur de camions de minerai
    public void IncrementTruckOreCounter()
    {
        counterTruck++;
        counterTruckOre++;
        if (counterTruckOre >= maxTruckOre)
        {
            EndGame();
        }
    }
}