using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



/*
 *  Fonctionnement du jeux et de la classe : 
 *  
 *      UsineAssemblageGameManager cr�� des Circuit imprime qui avance sur le tapis roulant seul
 *      Il les d�truits quand il sont hors de l'image
 *      Il g�re le nombre de circuit valid� et non valid� et Affiche le r�sultat dans l'UI
 *      G�re aussi la destruction des composants lach� dans le vide et l'appovisionnement de nouveau composant dans la palette.
 *      
 *      Le joueur clic sur une palette avec la souris pour avoir les composant et les dragAndDrop sur les circuits. 
 *      S'il les drop assez proche, les composant vont se loger au bonne endroit, sinon ils disparaissent
 *      Une fois attach� � un circuit, un composant le suit dans son movement jusqu'� la disparition du circuit.
 *      C'est le circuit qui dit � UsineAssemblageGameManager s'il est valid� au non.
 *      C'est le circuit qui d�truit les composants qui lui sont attach� au moment de sa d�struction.
 *      
 *      Le joueur valide un circuit s'il pose les composant au bonne endroit
 *      Plus le temps avance, plus les circuit avantce vite et les composant deviennent compliqu�.
 *      Le joueur gagne la partie s'il fait assez de circuit dans le temps impartie.
 *      
 */

public class UsineAssemblageGameManager : Singleton<UsineAssemblageGameManager>
{
    public GameObject[] tabComposant; //contient les diff�rents composant que le joueur pourra plac�
    public GameObject[] tabCircuitImprime; // concitient les diff�rent type de circuit avec les composant � plac� � des endroit diff�rent
    public Transform spawnPoint; // Point de d�part des circuits imprim�s
    public Transform endPoint; //A partir de ce point on est supos� hors de l'�cran
    public float circuitSpeed = 1f; // Vitesse des circuits sur le tapis
    public float timeLimit = 60f; // Temps limite pour terminer le jeu

    private float timeSinceLastSpawn = 0f;
    private float spawnInterval = 3f; // Intervalle de spawn (1 seconde)
    private bool userHasCliquedOnComponant = false;
    private GameObject currentComponent; // Composant actuellement tenu par le joueur



    [SerializeField] private int nbCircuitWin = 0;
    [SerializeField] private int nbCircuitLose = 0;
    [SerializeField] private int nbCircuitGoal = 10;
    [SerializeField] private float timeElapsed = 0f; //Temps �coul�
    [SerializeField] private bool isMouseDown = false;
    [SerializeField] private float clickThreshold = 50f; // Distance acceptable en pixels pour consid�rer un clic comme valide  


    private List<GameObject> lstCircuitInGame = new List<GameObject>();    //Contient des ref vers les objects circuit instenti�
    private List<GameObject> lstComposantInGame = new List<GameObject>(); //Contient des ref vers les objects composant que le joueur a cr��


    void Start()
    {
        SpawnCircuitImprime();
    }


    void Update()
    {
        // Calcul du temps �coul�
        timeElapsed += Time.deltaTime;

        // V�rifie si assez de temps s'est �coul� pour spawn un nouveau circuit
        if (Time.time - timeSinceLastSpawn >= spawnInterval)
        {
            SpawnCircuitImprime();
            timeSinceLastSpawn = Time.time; // R�initialise le dernier temps de spawn
        }

        //Pour Le jeux Usine Assemblage 
        if (Input.GetMouseButtonDown(0))
        {
            UsineAssemblageGameManager.Instance.UserClicLeftDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            UsineAssemblageGameManager.Instance.UserClicLeftUp();
        }



        //Si le joueur a un composant acroch� � sa sourie
        // Si un composant est en cours de suivi, ajuste sa position pour suivre la souris
        if (currentComponent != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Ajuste la profondeur pour s'assurer qu'il est visible
            currentComponent.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    void SpawnCircuitImprime()
    {
        int randomIndex = Random.Range(0, tabCircuitImprime.Length);
        GameObject newCircuit = Instantiate(tabCircuitImprime[randomIndex], spawnPoint.position, Quaternion.identity);
        lstCircuitInGame.Add(newCircuit);
    }


    public void UserClicLeftUp()
    {

    }

    public void UserClicLeftDown()
    {
        Vector3 mousePos = Input.mousePosition;
        int indiceComposant = 0;
        for (int i = 0; i<tabComposant.Length; i++)
        {
            // Convertit la position du GameObject en coordonn�es d'�cran  
            Vector3 screenPos = Camera.main.WorldToScreenPoint(tabComposant[i].transform.position);

            // Calcule la distance entre la position de la souris et la position du composant  
            float distance = Vector3.Distance(mousePos, screenPos);

            // Si la distance est dans le seuil d�fini  
            if (distance <= clickThreshold)
            {
                Debug.Log("Vous avez cliqu� sur : " + tabComposant[i].name);
                userHasCliquedOnComponant = true;
                indiceComposant = i;
                break;
            }
        }
        if(userHasCliquedOnComponant)
        {
            //ICI cpt = l'indice du composant qui a �t� clik�
            print("Le indiceComposant : " + indiceComposant);
            GameObject newComposant = Instantiate(tabComposant[indiceComposant], mousePos, Quaternion.identity);
            lstComposantInGame.Add(newComposant);
            currentComponent = newComposant;
        }
    }





    public void AddGoodCircuit()
    {
        this.nbCircuitWin += 1;
    }
    public void AddBadCircuit()
    {
        this.nbCircuitLose += 1;
    }
    public float GetActualSpeed()
    {
        return circuitSpeed;
    }
    public float GetEndPosition()
    {
        return endPoint.position.x;
    }
}
