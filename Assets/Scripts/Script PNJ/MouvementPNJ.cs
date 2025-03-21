using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Cette classe g�re les mouvements des PNJ
 * 
 * Il y a 3 types de PNJ � ce jour : 
 *      - les pnj nomm�     NORMAL           : MARCHE , IDLE
 *      - les pnj nomm�     SPECIFIQUE       : MARCHE , IDLE, ACTION_SPECIFIQUE
 *      - les pnj nomm�     IMOBILE          : IDLE
 * 
 * Tous les PNJ peuvent parler avec le joueur si ce dernier se rapproche assez
 * Au niveau des animations et des etats, les PNJ alterne les animations � tous de roles
 * 
 * Pour le d�placement, il y a un tableau de gameObject � rentr� dans chaque PNJ pour les d�placements
 * NOTE : Si quelqu'un veut faire pas bouger un PNJ, alors il a juste a ne mettre qu'un seul point
 * 
 * Si vous avez des remarques ou des questions, voir avec Yanis, le responssable des PNJ
 * 
 * Cordialement
 */

public class MouvementPNJ : MonoBehaviour
{
    public GameObject[] tabPointDestination; // Deux points pour le d�placement en boucle
    public Animator animator;

    [SerializeField] private int currentDestinationIndex = 0; // Index du point de destination actuel
    [SerializeField] private float vitesse = 2f;              // Vitesse de d�placement
    [SerializeField] private float idleDuration;              // Dur�e de pause en idle, en secondes
    private float stateTimer = 0f;                             

    [SerializeField] private float minIdleTime = 2f;   // Temps minimum en idle
    [SerializeField] private float maxIdleTime = 5f;   // Temps maximum en idle
    [SerializeField] private float minActionTime = 3f; // Temps minimum pour l'action sp�cifique
    [SerializeField] private float maxActionTime = 6f; // Temps maximum pour l'action sp�cifique

    private float timeIdle = 0f;
    private float timeAction = 0f;

    public enum PNJ_StateManif { manif, pacifiste }
    private PNJ_StateManif currentStateManif = PNJ_StateManif.pacifiste;
    private Transform[] tabPointDestinationManif = new Transform[2];
    private int currentDestinationIndexManif = 0;
    public bool PNJHasArrivedToManifPoint = false; //pour emp�cher de faire un calcule des milier de fois
    private bool hasArrived = false;
    private float EcartToDestination = 0.0f; //pour que tous les PNJ ne s'arr�te pas � la m�me ligne



    private enum PNJState { Idle, ActionSpecific, Moving }
    private PNJState currentState = PNJState.Idle;

    public enum PNJType { NORMAL, SPECIFIQUE, IMOBILE }
    public PNJType typePNJ; //pour savoir de quelle type il est. Tr�s importan !!!!!

    private bool ThisPnjSpeakToPlayer = false;
    public Vector3 initialScale;  // Pour sauvegarder la taille initiale du PNJ


    /*pour la gestio nde l'audio avec diff�rence corbeau, poul, pnj*/
    public enum TypeAudioPnj
    {
        PNJ,
        Corbeau,
        Poule
    }
    public TypeAudioPnj typeAudioPnj = TypeAudioPnj.PNJ;

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
        if (ThisPnjSpeakToPlayer && currentState != PNJState.Idle)
            SetState(PNJState.Idle);

        if (ThisPnjSpeakToPlayer == true)
            return;

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
            Transform targetPoint = tabPointDestinationManif[currentDestinationIndexManif];

            Vector3 targetPosition = new Vector3(targetPoint.position.x - EcartToDestination, transform.position.y, targetPoint.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, vitesse * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f && !hasArrived)
            {
                hasArrived = true;
                ManifManager.Instance.PNJArrived();
                print("Arrived" + gameObject.name);
                SetState(PNJState.Idle);
            }
        }
        else
        {
            // Destination actuelle
            Transform targetPoint = tabPointDestination[currentDestinationIndex].transform;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, vitesse * Time.deltaTime);

            // V�rifie si la destination est atteinte
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                currentDestinationIndex = (currentDestinationIndex + 1) % tabPointDestination.Length; // Passer au point suivant
                ChosseNextState();
            }
        }
    }

    private void UpdateSpriteDirection()
    {
        if (currentStateManif == PNJ_StateManif.manif)
        {
            Transform targetPoint = tabPointDestinationManif[currentDestinationIndexManif].transform;
            float direction = targetPoint.position.x > transform.position.x ? 1 : -1;
            transform.localScale = new Vector3(initialScale.x * direction, initialScale.y, initialScale.z);  // Applique la direction tout en gardant la taille initiale    }
        }
        else
        {
            Transform targetPoint = tabPointDestination[currentDestinationIndex].transform;
            float direction = targetPoint.position.x > transform.position.x ? 1 : -1;
            transform.localScale = new Vector3(initialScale.x * direction, initialScale.y, initialScale.z);  // Applique la direction tout en gardant la taille initiale    }
        }
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
            {
                //pour que je corbeau jou son son
                if(typeAudioPnj == TypeAudioPnj.Corbeau)
                {
                        //AudioManager.Instance.PlaySoundEffet(AudioType.Corbeau);
                        int chanceDeParler = Random.Range(0, 5);
                        AudioSource hh = GetComponent<AudioSource>();
                        AudioClip clip = AudioManager.Instance.GetClip(AudioType.Corbeau);
                        hh.clip = clip;
                        hh.Play();
                }

                if(typeAudioPnj == TypeAudioPnj.Poule)
                {
                    //AudioManager.Instance.PlaySoundEffet(AudioType.Poule1);
                    int chanceDeParler = Random.Range(0, 5);
                    AudioSource hh = GetComponent<AudioSource>();

                    AudioClip clip = AudioManager.Instance.GetClip(AudioType.Poule1);
                    hh.clip = clip;

                    if (chanceDeParler == 0 && hh != null)
                        hh.Play();
                }

                animator.SetTrigger("idl");
                break;

            }
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


    //pour que le ManifManager changer l'�tat du pnj en manifestant quand on est dans la scene de manifestation
    public void SetStateManif(Transform ptsDes, Transform ptsDepart, float v, float ecart)
    {
        currentStateManif = PNJ_StateManif.manif;
        tabPointDestinationManif[1] = ptsDes;
        tabPointDestinationManif[0] = ptsDepart;
        vitesse = v;
        EcartToDestination = ecart;
    }

    //Pour changer la destination en mode manif
    public void ChangeDestinationManif()
    {
        currentDestinationIndexManif = (currentDestinationIndexManif == 0) ? 1 : 0;

        PNJHasArrivedToManifPoint = false;
        hasArrived = false;
        SetState(PNJState.Moving);
    }

    public void changeToWlaking()
    {
        hasArrived = false;

        SetState(PNJState.Moving);
        UpdateSpriteDirection();
    }

    public bool HasArrivedAtDestination() => hasArrived;

}
