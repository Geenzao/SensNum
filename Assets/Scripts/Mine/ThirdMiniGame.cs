using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ThirdMiniGame : Singleton<ThirdMiniGame>
{
    public event Action ThirdMiniGameFinish;

    // Variables privées pour les compteurs et les états du jeu
    private int counterTruck = 0;
    private int counterTruckOre = 0;
    private int maxTruckOre = 3;

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