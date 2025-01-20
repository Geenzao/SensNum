using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ThirdMiniGame : Singleton<ThirdMiniGame>
{
    public event Action ThirdMiniGameFinish;

    // Variables privées pour les compteurs et les états du jeu
    public int counterTruck = 0;
    public int counterTruckOre = 0;
    private int maxTruckOre = 3;
    // tableau qui contient la position de chaque gameobject camions en glissant le gameobject camion dans le tableau
    public GameObject[] truckPosition;


    // Méthode pour incrémenter le compteur de camions
    public void IncrementTruckCounter()
    {
        counterTruck++;
    }

    // Méthode pour incrémenter le compteur de camions de minerai
    public void IncrementTruckOreCounter()
    {
        counterTruck++;
        counterTruckOre++;
        if (counterTruckOre >= maxTruckOre)
        {
            ThirdMiniGameFinish?.Invoke();
        }
    }

    //geter pour counterTruck
    public int CounterTruck
    {
        get { return counterTruck; }
    }

    //geter pour counterTruckOre
    public int CounterTruckOre
    {
        get { return counterTruckOre; }
    }
}