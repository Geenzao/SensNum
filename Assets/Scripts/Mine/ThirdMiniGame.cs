using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ThirdMiniGame : Singleton<ThirdMiniGame>
{
    public event Action ThirdMiniGameFinish;

    // Variables priv�es pour les compteurs et les �tats du jeu
    private int counterTruck = 0;
    private int counterTruckOre = 0;
    private int maxTruckOre = 3;

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