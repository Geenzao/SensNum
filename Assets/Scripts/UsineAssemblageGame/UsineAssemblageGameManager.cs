using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using Unity.VisualScripting;


//Classe Utils pour faire la gestion d'un rectangle et savoir si un point est � l'interieur
public class Rectangle
{
    public float xMin, yMin, width, height;
    public float xMax, yMax;

    public Rectangle(float xMin, float yMin, float width, float height)
    {
        this.xMin = xMin;
        this.yMin = yMin;
        this.width = width;
        this.height = height;

        xMax = width + xMin;
        yMax = height + yMin;
    }

    public bool isInside(Vector3 other)
    {
        return other.x >= xMin && other.x < xMax && other.y >= yMin && other.y < yMax;
    }
    public bool isInside(Vector2 other)
    {
        return other.x >= xMin && other.x < xMax && other.y >= yMin && other.y < yMax;
    }
}


/*
 *  Fonctionnement du jeux et de la classe : 
 *  
 *      UsineAssemblageGameManager cr�� des Circuit imprime qui avance sur le tapis roulant tout seul
 *      Il les d�truits quand il sont hors de l'image
 *      Gere un chronometre en interne pour g�r� le jeux/fin de la partie/Annalyse des r�sultats
 *      G�re aussi la destruction des composants lach� dans le vide et l'appovisionnement de nouveau composant dans la palette.
 *      
 *      Le joueur clic sur une palette avec la souris pour avoir les composant et les dragAndDrop sur les circuits. 
 *      S'il les drop assez proche, les composant vont se loger au bonne endroit, sinon ils disparaissent
 *      Le joueur doit completer un minium de circuit dans le temps impartie sinon il pert
 *      Le joueur peut rejouer une parti une fois qu'il a perdu ou gagnier
 *      
 *      C'est les circuits qui disent � UsineAssemblageGameManager s'ils sont valid� au non juste avant d'�tre d�truit
 *      C'est le circuit qui d�truit les composants qui lui sont attach� au moment de sa d�struction.
 *      
 *      Le scripte UsineAssemblageUIManager g�re l'UI, les menus pour le minu jeux. 
 *      Il y a un enum pour g�r� les diff�rent Etat du jeux.
 *      
 *      Les circuits acc�l�re avec le temps 
 *      
 */



/*
 * TODO : am�lior� l'UI
 */


public class UsineAssemblageGameManager : Singleton<UsineAssemblageGameManager>
{
    [Header("Tableau des composants")]
    public GameObject[] tabComponent; //contient les diff�rents composant que le joueur pourra plac�
    [Header("Tableau des circuits")]
    public GameObject[] tabCircuitImprime; // concitient les diff�rent type de circuit avec les composant � plac� � des endroit diff�rent

    [Header("Position Spawn/destroy")]
    public Transform spawnPoint; // Point de d�part des circuits imprim�s
    public Transform endPoint; //A partir de ce point on est supos� hors de l'�cran
    public Transform spawnInterval; //a partir de ce point, on fait spawn un autre circuit

    [Header("Param�tre du jeu")]
    public float circuitSpeed = 1.0f; // Vitesse des circuits sur le tapis
    public float timeLimit = 60f; // Temps limite pour terminer le jeu

    //pour l'acceleration du jeux
    public float accelerationInterval = 5.0f; //temps en seconde entre deux acceleration des circuits
    public float speedAcceleration = 1.0f; //acceletation en float � chaque step

    private float timeSinceLastAcceleration = 0.0f;
    private float timeElapsed = 0f; //Temps �coul�
    private float secondRemaining = 0; //temps en seconde restant avant la fin du jeux
    private float timeGoal = 0; //Permet d'enregistrer le temps que met le joueur a acomplir l'objectif
    private float SpeedCircuitSave = 0;

    private bool userHasCliquedOnComponent = false;
    private GameObject currentComponent; // Composant actuellement tenu par le joueur
    private int indexCurrentComponent;

    [Header("Donn�e du jeux")]
    [SerializeField] private int nbCircuitWin = 0;
    [SerializeField] private int nbCircuitLose = 0;
    [SerializeField] private int nbCircuitGoal = 5;


    private List<GameObject> lstCircuitInGame = new List<GameObject>();    //Contient des ref vers les objects circuit instenti�

    private UsineAssemblageUI UsineAssemblageUI;

    private bool playerHasWin = false;

    private Coroutine coroutineNotifyAcceleration = null;

    void Start()
    {
        UsineAssemblageUI = GameObject.Find("AssemblyGameUI").GetComponent<UsineAssemblageUI>();
        secondRemaining = timeLimit;
        SpeedCircuitSave = circuitSpeed; //parce que circuitSpeed est amener � changer dans la game 
        //On fait spawn un premier circuit pour que le joueur n'attende pas
        SpawnCircuitImprime();

        StopGame(); //on arr�te le jeux au d�but
    }


    void Update()
    {
        // Gestion du chronom�tre
        if (Time.timeScale > 0 && secondRemaining > 0)
        {
            timeElapsed += Time.deltaTime;
            secondRemaining = Mathf.Max(0, timeLimit - timeElapsed); // �vite les valeurs n�gatives
            UsineAssemblageUI.UpdateTimeRemaining((int)secondRemaining);

            //Si assez de temps c'est �coul�, on acc�l�re les circuit pour augmenter la dificult�
            if (timeElapsed - timeSinceLastAcceleration >= accelerationInterval)
            {
                IncreaseCircuitSpeed();
                timeSinceLastAcceleration = timeElapsed;
            }

            // V�rification de la fin du jeu
            if (secondRemaining <= 0 || playerHasWin)
            {
                //Si le jeux se termine et on a toujours un composant dans la main, ce dernier ce d�truit
                if (currentComponent != null)
                    Destroy(currentComponent);
                currentComponent = null;

                AnnalyseGame();
            }
        }

        // Si un composant est en cours de suivi, ajuste sa position pour suivre la souris
        if (currentComponent != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Ajuste la profondeur pour s'assurer qu'il est visible
            currentComponent.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    //pour faire appara�tre un nouveau circuit
    public void SpawnCircuitImprime()
    {
        int randomIndex = UnityEngine.Random.Range(0, tabCircuitImprime.Length);

        // S�lectionner un angle de rotation al�atoire parmi 0, 90, 180, et 270 degr�s pour l'appliquer au circuit
        int[] rotationAngles = { 0, 90, 180, 270 };
        int randomAngle = rotationAngles[UnityEngine.Random.Range(0, rotationAngles.Length)];
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle);


        GameObject newCircuit = Instantiate(tabCircuitImprime[randomIndex], spawnPoint.position, randomRotation);
        if (newCircuit.GetComponent<Renderer>() != null)
            lstCircuitInGame.Add(newCircuit);
        else
            Debug.LogError("Le CircuitImprime instanci� n'a pas de Renderer.");
    }


    //pour accelerer la vitesse des circuits
    void IncreaseCircuitSpeed()
    {
        //On actualise la liste pour suprimer les circuit suprime
        actualiseListeCircuitInGame();
        foreach (GameObject go in lstCircuitInGame)
        {
            //On accel�re les circuits
            go.gameObject.GetComponent<CircuitImprime>().addSpeed(speedAcceleration);
        }

        //A partir de maintenant la vitesse de base augmentes
        circuitSpeed += speedAcceleration;

        //On montre au joueur qu'il y a eut une acceleration
        if (this.coroutineNotifyAcceleration != null)
        {
            StopCoroutine(coroutineNotifyAcceleration);
        }
        coroutineNotifyAcceleration = StartCoroutine(StartNotifyAccelerationCoroutine());
        Debug.LogWarning("On accelere");
    }

    IEnumerator StartNotifyAccelerationCoroutine()
    {
        UsineAssemblageUI.ShowNotifyAcceleration();
        yield return new WaitForSeconds(0.5f);
        UsineAssemblageUI.HideNotifyAcceleration();
    }


    //ICI on regarde si le composant est lach� sur un circuit
    //si oui, on regarde quelle est l'endroit le plus proche au niveau des place que le circuit a 
    //et on d�truit le composant
    public void UserClicLeftUp()
    {
        //On execute la suite seulement si le joueur a un composant dans la main
        if (userHasCliquedOnComponent == false || currentComponent == null)
            return;

        //pour mettre a jour la liste
        //lstCircuitInGame.RemoveAll(item => item == null);
        actualiseListeCircuitInGame();

        int indexCircuit = -1;

        for (int i = 0; i < lstCircuitInGame.Count; i++)
        {
            // Obtenir la taille du GameObject (en �cran)
            Renderer renderer = lstCircuitInGame[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                // Obtenir un rectangle qui encompasse le GameObject  
                Vector2 size = renderer.bounds.size;
                Vector2 center = lstCircuitInGame[i].transform.position;

                Rectangle rect = new Rectangle(center.x - size.x / 2,
                                        center.y - size.y / 2,
                                        size.x,
                                        size.y);

                if (rect.isInside(currentComponent.transform.position))
                {
                    //Si on est l�, c'est qu'on est dans un circuit imprim� et on sais lequelle
                    //Debug.Log("Circuit trouve : " + lstCircuitInGame[i].name);
                    indexCircuit = i;
                    break;
                }
            }
            else
                Debug.LogWarning("Renderer non trouv� pour l'objet � l'indice " + i);
        }

        //Le joueur a bien plac� le composant, reste � savoir a quelle place dans le circuit.
        if (indexCircuit != -1)
        {
            List<ComponentPlace> lstTempo = lstCircuitInGame[indexCircuit].GetComponent<CircuitImprime>().lstComponentPlaceOnCircuit;
            float distanceMin = float.MaxValue;
            int goodIndexComposentPlace = 0;
            for (int i = 0; i < lstTempo.Count; i++)
            {
                if (lstTempo[i].component == null)
                {
                    Debug.LogWarning("Le component ou le currentComponent est null");
                }

                float distance = Vector3.Distance(lstTempo[i].component.transform.position, currentComponent.transform.position);

                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    goodIndexComposentPlace = i;
                }
            }

            //ici on a goodIndexComposantPlace qui est �gal � la bonne place dans le circuit
            //On teste alors si c'est le bon composant qui va au bonne endroit. Si oui c'est bon.
            lstCircuitInGame[indexCircuit].GetComponent<CircuitImprime>().FillComposantPlace(currentComponent.GetComponent<ComponentCircuit>().type, goodIndexComposentPlace);
        }
        Destroy(currentComponent);
        currentComponent = null;
        userHasCliquedOnComponent = false;
    }
    public void UserClicLeftDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        indexCurrentComponent = 0;
        for (int i = 0; i < tabComponent.Length; i++)
        {
            // Obtenir la taille du GameObject (en �cran)
            Renderer renderer = tabComponent[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                // Obtenir un rectangle qui encompasse le GameObject  
                Vector2 size = renderer.bounds.size;
                Vector2 center = tabComponent[i].transform.position;

                Rectangle rect = new Rectangle(center.x - size.x / 2,
                                      center.y - size.y / 2,
                                      size.x,
                                      size.y);

                if (rect.isInside(mousePos))
                {
                    //Si on est l�, c'est qu'on est dans un circuit imprim� et on sais lequelle
                    //Debug.Log("Circuit trouve : " + tabComponent[i].name);
                    userHasCliquedOnComponent = true;
                    indexCurrentComponent = i;
                    break;
                }
            }
            else
                Debug.LogWarning("Renderer non trouv� pour l'objet � l'indice " + i);
        }

        if (userHasCliquedOnComponent == true)
        {
            //ICI cpt = l'indice du composant qui a �t� clik�
            //print("Le indiceComposant : " + indiceComposant);
            GameObject newComposant = Instantiate(tabComponent[indexCurrentComponent], mousePos, Quaternion.identity);
            currentComponent = newComposant;
        }
    }

    public void AddGoodCircuit()
    {
        this.nbCircuitWin += 1;

        //Si le joueur a r�alis� l'objectif, on note son temps et on arr�te le jeux
        if (nbCircuitWin >= nbCircuitGoal)
        {
            timeGoal = timeElapsed;
            playerHasWin = true;
        }

        UpdateUI();
    }

    private void actualiseListeCircuitInGame()
    {
        //pour mettre a jour la liste
        lstCircuitInGame.RemoveAll(item => item == null);
    }

    public void AddBadCircuit()
    {
        this.nbCircuitLose += 1;
        UpdateUI();
    }

    //Pour actualis� l'UI du nombre de circuit fait
    public void UpdateUI()
    {
        UsineAssemblageUI.UbdateUI();
    }

    //Pour lancer la game une fois que le joueur a lu les regles
    public void RunGame()
    {
        Time.timeScale = 1.0f;
    }
    //pour stoper la game au moment des menus
    public void StopGame()
    {
        Time.timeScale = 0.0f;
    }


    //On appel cette fonction � la fin d'une game quand le temps est fini 0
    public void AnnalyseGame()
    {
        StopGame();

        if (nbCircuitWin >= nbCircuitGoal/* && timeElapsed < timeLimit*/)
        {
            //Le joueur a ganier
            UsineAssemblageUI.PlayerHasWin();
        }
        else
        {
            //Il a perdu
            UsineAssemblageUI.PlayerHasLose();
        }
    }

    public void InitialiseGame()
    {
        // 1. R�initialisation des variables de jeu
        nbCircuitWin = 0;
        nbCircuitLose = 0;
        nbCircuitGoal = 10;
        timeElapsed = 0f;
        secondRemaining = timeLimit;
        timeSinceLastAcceleration = 0f;

        timeGoal = 0f;
        playerHasWin = false;
        circuitSpeed = SpeedCircuitSave;

        // 2. Suppression des objets actifs dans le jeu
        // Supprimer tous les circuits imprim�s encore pr�sents
        foreach (var circuit in lstCircuitInGame)
        {
            if (circuit != null)
            {
                Destroy(circuit);
            }
        }
        lstCircuitInGame.Clear();

        // Supprimer le composant actuellement tenu par le joueur (s'il existe)
        if (currentComponent != null)
        {
            Destroy(currentComponent);
            currentComponent = null;
        }
        userHasCliquedOnComponent = false;

        // 3. Mettre � jour l'interface utilisateur
        UsineAssemblageUI.UpdateTimeRemaining((int)secondRemaining);
        UsineAssemblageUI.UbdateUI();

        // 3.1 Ajouter un circuit
        SpawnCircuitImprime();

        // 4. Mettre le jeu en pause pour attendre le lancement
        StopGame();

        //Debug.Log("Jeu initialis� avec succ�s !");
    }

    //Getter
    public int GetNbCircuitWin() { return nbCircuitWin; }
    public int GetNbCircuitLose() { return nbCircuitLose; }
    public int GetNbCircuitGoal() { return nbCircuitGoal; }
    public int GetTimeForGoal() { return (int)timeGoal; }
    public float GetActualSpeed() { return circuitSpeed; }
    public float GetEndPosition() { return endPoint.position.x; }
    public float GetSpawnInterval() { return spawnInterval.position.x; }
}