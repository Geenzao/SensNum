using UnityEngine;
using UnityEngine.UI;


public class CandyGameManager : MonoBehaviour
{
    public CandyGameManager Instance;

    public GameObject backgroundPanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    public GameObject alerte;

    public Text nbSuperMatchsText;
    public Text nbMatchsText;
    public Text pointText;

    public Image barredechet;

    public float nbDechets = 0;
    public int nbSuperMatchs = 0;
    public int nbMatchs = 0;
    public int points;

    public bool isGameEnded;

    private void Awake()
    {
        Instance = this;
    }

    float tempsDerniereExecution = 0.0f; // stock le temps passé depuis la derniere execution;
    float delai = 5.0f;    // tu defini l'interval voulu, en seconde.	

    void Update()
    {
        if (nbDechets < 15)
        {
            if(nbDechets < 10)
                alerte.SetActive(false);
            tempsDerniereExecution += Time.fixedDeltaTime;  // ajoute a chaque update le temps écoulé depuis le dernier Update		
            if (tempsDerniereExecution > delai)
            {
                MonAction();
                tempsDerniereExecution = 0;
            }

        }
        if (nbDechets == 15) 
            LoseGame();
    }
    void MonAction()
    {
        nbDechets++;
        if (nbDechets < 11)
        {
            barredechet.fillAmount = nbDechets / 10f;
        }
        else alerte.SetActive(true);
    }
    public void Initialize()
    {

    }
    void Start()
    {
        
    }

    public void ProcessTurn(int pointGain,int coupRealise)
    {
        points += pointGain;
        pointText.text = points.ToString();

        if (coupRealise == 1)
        {
            nbMatchs++;
            if(nbDechets > 0)
            {
                nbDechets-=1;
            }
            nbMatchsText.text = nbMatchs.ToString();
        }
        if (coupRealise == 2)
        {
            nbSuperMatchs++;
            if (nbDechets > 0)
            {
                nbDechets-=5;
            }
            nbSuperMatchsText.text = nbMatchs.ToString();
        }
        if (nbDechets < 11)
        {
            barredechet.fillAmount = nbDechets / 10f;
        }

    }

    public void WinGame()
    {
        
    }
    public void LoseGame()
    {
       defeatPanel.SetActive(true);
    }
}
