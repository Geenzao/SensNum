using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Cette classe gère les mouvements des PNJ
 * 
 * Il y a 3 types de PNJ à ce jour : 
 *      - les pnj nommé     NORMAL           : MARCHE , IDLE
 *      - les pnj nommé     SPECIFIQUE       : MARCHE , IDLE, ACTION_SPECIFIQUE
 *      - les pnj nommé     IMOBILE          : IDLE
 * 
 * Tous les PNJ peuvent parler avec le joueur si ce dernier se rapproche assez
 * Au niveau des animations et des etats, les PNJ alterne les animations à tous de roles
 * 
 * Pour le déplacement, il y a un tableau de gameObject à rentré dans chaque PNJ pour les déplacements
 * NOTE : Si quelqu'un veut faire pas bouger un PNJ, alors il a juste a ne mettre qu'un seul point
 * 
 * Si vous avez des remarques ou des questions, voir avec Yanis, le responssable des PNJ
 * 
 * Cordialement
 */

public class MouvementPNJ : MonoBehaviour
{
    public GameObject[] tabPointDestination; // Deux points pour le déplacement en boucle
    public Animator animator;

    [SerializeField] private int currentDestinationIndex = 0; // Index du point de destination actuel
    [SerializeField] private bool isWalking = true;           // Contrôle si le PNJ marche ou est en idle
    [SerializeField] private float vitesse = 2f;              // Vitesse de déplacement
    [SerializeField] private float idleDuration;              // Durée de pause en idle, en secondes
    private float stateTimer = 0f;                             

    [SerializeField] private float minIdleTime = 2f;   // Temps minimum en idle
    [SerializeField] private float maxIdleTime = 5f;   // Temps maximum en idle
    [SerializeField] private float minActionTime = 3f; // Temps minimum pour l'action spécifique
    [SerializeField] private float maxActionTime = 6f; // Temps maximum pour l'action spécifique

    private float timeIdle = 0f;
    private float timeAction = 0f;

    public enum PNJ_StateManif { manif, pacifiste }
    private PNJ_StateManif currentStateManif = PNJ_StateManif.pacifiste;
    private Transform[] tabPointDestinationManif;
    private int currentDestinationIndexManif = 0;


    private enum PNJState { Idle, ActionSpecific, Moving }
    private PNJState currentState = PNJState.Idle;

    public enum PNJType { NORMAL, SPECIFIQUE, IMOBILE }
    public PNJType typePNJ; //pour savoir de quelle type il est. Très importan !!!!!

    private bool ThisPnjSpeakToPlayer = false;
    public Vector3 initialScale;  // Pour sauvegarder la taille initiale du PNJ

    void Awake()
    {
        initialScale = gameObject.transform.localScale;  // Sauvegarde la taille initiale
    }

    void Start()
    {
        initialScale = gameObject.transform.localScale;
        timeIdle = GetRandomIdleTime();
        timeAction = GetRandomActionTime();
        SetState(PNJState.Idle);

        //pour la manif
        currentDestinationIndexManif = 0;

        // Positionne le PNJ au premier point de destination
        if (tabPointDestination.Length > 0)
        {
            //On place corectement le PNJ, sur le premier point
            transform.position = tabPointDestination[0].transform.position;
        }
        else
        {
            typePNJ = PNJType.IMOBILE; //pour s'assurer que le jeux ne pete pas
        }
    }

    void Update()
    {
        if (ThisPnjSpeakToPlayer)
            SetState(PNJState.Idle);

        if(currentStateManif == PNJ_StateManif.manif)
        {
            MoveToNextDestination();
        }
        else
        {
            stateTimer += Time.deltaTime;

            switch (currentState)
            {
                case PNJState.Idle:
                    if (stateTimer >= timeIdle)
                    {
                        ChosseNextState();
                    }
                    break;

                case PNJState.ActionSpecific:
                    if (stateTimer >= timeAction)
                    {
                        ChosseNextState();
                    }
                    break;

                case PNJState.Moving:
                    MoveToNextDestination();
                    break;
            }
        }
    }

    private void MoveToNextDestination()
    {
        if(currentStateManif == PNJ_StateManif.manif)
        {
            // Point cible à atteindre
            Transform targetPoint = tabPointDestinationManif[currentDestinationIndexManif];

            // Garde la position Y actuelle (évite que l'objet change de hauteur)
            Vector3 targetPosition = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);

            // Déplace l'objet vers la cible
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, vitesse * Time.deltaTime);

            // Vérifie si la destination est atteinte
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetState(PNJState.Idle);
            }
        }
        else
        {
            // Destination actuelle
            Transform targetPoint = tabPointDestination[currentDestinationIndex].transform;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, vitesse * Time.deltaTime);

            // Vérifie si la destination est atteinte
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                currentDestinationIndex = (currentDestinationIndex + 1) % tabPointDestination.Length; // Passer au point suivant
                ChosseNextState();
            }
        }
    }

    private void UpdateSpriteDirection()
    {
        Transform targetPoint = tabPointDestination[currentDestinationIndex].transform;
        float direction = targetPoint.position.x > transform.position.x ? 1 : -1;
        transform.localScale = new Vector3(initialScale.x * direction, initialScale.y, initialScale.z);  // Applique la direction tout en gardant la taille initiale    }
    }

    private void ChosseNextState()
    {
        switch (typePNJ)
        {
            case PNJType.NORMAL:
                SetState(currentState == PNJState.Moving ? PNJState.Idle : PNJState.Moving);
                break;
            case PNJType.SPECIFIQUE:
                SetState(currentState == PNJState.Moving ? PNJState.ActionSpecific : currentState == PNJState.ActionSpecific ? PNJState.Idle : PNJState.Moving);
                break;
            case PNJType.IMOBILE:
                SetState(PNJState.Idle);
                break;
        }
    }

    private void SetState(PNJState newState)
    {
        currentState = newState;
        stateTimer = 0f;
        switch (currentState)
        {
            case PNJState.Idle:
                animator.SetTrigger("idl");
                break;
            case PNJState.ActionSpecific:
                animator.SetTrigger("interaction");
                break;
            case PNJState.Moving:
                animator.SetTrigger("walk");
                UpdateSpriteDirection();
                break;
        }
    }

    public void PnjTalk() { ThisPnjSpeakToPlayer = true; }
    public void PnjDontTalk() { ThisPnjSpeakToPlayer = false; }

    private float GetRandomIdleTime() => Random.Range(minIdleTime, maxIdleTime);
    private float GetRandomActionTime() => Random.Range(minActionTime, maxActionTime);


    //pour que le ManifManager changer l'état du pnj en manifestant quand on est dans la scene de manifestation
    public void SetStateManif(Transform ptsDes, Transform ptsDepart)
    {
        currentStateManif = PNJ_StateManif.manif;
        tabPointDestinationManif[0] = ptsDes;
        tabPointDestinationManif[0] = ptsDepart;
    }

    //Pour changer la destination en mode manif
    public void ChangeDestinationManif()
    {
        if (currentDestinationIndexManif == 0)
            currentDestinationIndexManif = 1;
        else
            currentDestinationIndexManif = 0;

        SetState(PNJState.Moving);
        
    }
}
