using UnityEngine;
using System;

public class SecondMiniGame : Singleton<SecondMiniGame>
{
    public event Action SecondMiniGameFinish;

    // Variables priv�es pour les compteurs et les �tats du jeu
    private int cptOr = 0;
    private int cptCu = 0;
    private int cptLi = 0;

    // M�thode pour incr�menter le compteur d'or
    public void IncrementGoldCounter()
    {
        cptOr++;
    }

    // M�thode pour incr�menter le compteur de cuivre
    public void IncrementCopperCounter()
    {
        cptCu++;
    }

    // M�thode pour incr�menter le compteur de lithium
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