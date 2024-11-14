using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


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
    public GameObject[] tabComponent; //contient les diff�rents composant que le joueur pourra plac�
    public GameObject[] tabCircuitImprime; // concitient les diff�rent type de circuit avec les composant � plac� � des endroit diff�rent
    
    public Transform spawnPoint; // Point de d�part des circuits imprim�s
    public Transform endPoint; //A partir de ce point on est supos� hors de l'�cran
    
    public float circuitSpeed = 0.4f; // Vitesse des circuits sur le tapis
    public float timeLimit = 60f; // Temps limite pour terminer le jeu

    private float timeSinceLastSpawn = 0f;
    private float spawnInterval = 4f;
    private bool userHasCliquedOnComponent = false;
    private GameObject currentComponent; // Composant actuellement tenu par le joueur
    private int indexCurrentComponent;


    private int nbCircuitWin = 0;
    private int nbCircuitLose = 0;
    private int nbCircuitGoal = 10;
    private float timeElapsed = 0f; //Temps �coul�
    private float clickThreshold = 50f; // Distance acceptable en pixels pour consid�rer un clic comme valide  


    private List<GameObject> lstCircuitInGame = new List<GameObject>();    //Contient des ref vers les objects circuit instenti�
    private List<GameObject> lstComposantInGame = new List<GameObject>(); //Contient des ref vers les objects composant que le joueur a cr��


    public TextMeshProUGUI txtNbCircuitWin;
    public TextMeshProUGUI txtNbCircuitLose;
    public TextMeshProUGUI txtNbCircuitGoal;

    void Start()
    {
        SpawnCircuitImprime();
        txtNbCircuitWin.text = nbCircuitWin.ToString();
        txtNbCircuitGoal.text = nbCircuitGoal.ToString();
        txtNbCircuitLose.text = nbCircuitLose.ToString();
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
            //print("Clic Down");
            UsineAssemblageGameManager.Instance.UserClicLeftDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            //print("Clic Up");
            UsineAssemblageGameManager.Instance.UserClicLeftUp();
        }


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
        if (newCircuit.GetComponent<Renderer>() != null)
            lstCircuitInGame.Add(newCircuit);
        else
            Debug.LogError("Le CircuitImprime instanci� n'a pas de Renderer.");
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
        lstCircuitInGame.RemoveAll(item => item == null);

        int indexCircuit = -1;
       
        for (int i = 0; i < lstCircuitInGame.Count; i++)
        {
            if (lstCircuitInGame[i] != null)
            {
                // Obtenir la taille du GameObject (en �cran)
                Renderer renderer = lstCircuitInGame[i].GetComponent<Renderer>();
                if(renderer != null)
                {
                    // Obtenir un rectangle qui encompasse le GameObject  
                    Vector2 size = renderer.bounds.size;
                    Vector2 center = lstCircuitInGame[i].transform.position;

                    Rectangle rect = new Rectangle(center.x - size.x / 2,
                                          center.y - size.y / 2,
                                          size.x,
                                          size.y);

                    if(rect.isInside(currentComponent.transform.position))
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
            else
                Debug.LogWarning("lstCircuitInGame[" + i + "] est nul");
        }


         //Le joueur a bien plac� le composant, reste � savoir a quelle place dans le circuit.
        if(indexCircuit != -1)
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
            lstCircuitInGame[indexCircuit].GetComponent<CircuitImprime>().FillComposantPlace(currentComponent.GetComponent<Component>().type, goodIndexComposentPlace);
        }
        Destroy(currentComponent);
        currentComponent = null;
        userHasCliquedOnComponent = false;
    }

    public void UserClicLeftDown()
    {
        Vector3 mousePos = Input.mousePosition;
        indexCurrentComponent = 0;
        for (int i = 0; i<tabComponent.Length; i++)
        {
            // Convertit la position du GameObject en coordonn�es d'�cran  
            Vector3 screenPos = Camera.main.WorldToScreenPoint(tabComponent[i].transform.position);

            // Calcule la distance entre la position de la souris et la position du composant  
            float distance = Vector3.Distance(mousePos, screenPos);

            // Si la distance est dans le seuil d�fini
            if (distance <= clickThreshold)
            {
                //Debug.Log("Vous avez cliqu� sur : " + tabComposant[i].name);
                userHasCliquedOnComponent = true;
                indexCurrentComponent = i;
                break;
            }
        }
        if(userHasCliquedOnComponent == true)
        {
            //ICI cpt = l'indice du composant qui a �t� clik�
            //print("Le indiceComposant : " + indiceComposant);
            GameObject newComposant = Instantiate(tabComponent[indexCurrentComponent], mousePos, Quaternion.identity);
            lstComposantInGame.Add(newComposant);
            currentComponent = newComposant;
        }
    }



    public void AddGoodCircuit()
    {
        this.nbCircuitWin += 1;
        UpdateUI();
    }
    public void AddBadCircuit()
    {
        this.nbCircuitLose += 1;
        UpdateUI();
    }
    public float GetActualSpeed()
    {
        return circuitSpeed;
    }
    public float GetEndPosition()
    {
        return endPoint.position.x;
    }

    public void UpdateUI()
    {
        txtNbCircuitWin.text = nbCircuitWin.ToString();
        txtNbCircuitGoal.text = nbCircuitGoal.ToString();
    }
}
