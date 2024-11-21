using UnityEngine;
using System;

public class SecondMiniGame : Singleton<SecondMiniGame>
{
    public event Action SecondMiniGameFinish;

    // Variables privées pour les compteurs et les états du jeu
    private int cptOr = 0;
    private int cptCu = 0;
    private int cptLi = 0;

    // Méthode pour incrémenter le compteur d'or
    public void IncrementGoldCounter()
    {
        cptOr++;
    }

    // Méthode pour incrémenter le compteur de cuivre
    public void IncrementCopperCounter()
    {
        cptCu++;
    }

    // Méthode pour incrémenter le compteur de lithium
    public void IncrementLithiumCounter()
    {
        cptLi++;
    }

    // Getter pour cptOr
    public int CptOr
    {
        get { return cptOr; }
    }

    // Getter pour cptCu
    public int CptCu
    {
        get { return cptCu; }
    }

    // Getter pour cptLi
    public int CptLi
    {
        get { return cptLi; }
    }
}