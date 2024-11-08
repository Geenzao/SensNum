using System.Collections;
using UnityEngine;

// Ce script d�place les PNJ en suivant un parcours aller-retour entre deux points, avec des pauses obligatoires en mode idle.

public class MouvementPNJ : MonoBehaviour
{
    public GameObject[] tabPointDestination; // Deux points pour le d�placement en boucle
    public Animator animator;

    [SerializeField] private int currentDestinationIndex = 0; // Index du point de destination actuel
    [SerializeField] private bool isWalking = true;           // Contr�le si le PNJ marche ou est en idle
    [SerializeField] private float vitesse = 2f;              // Vitesse de d�placement
    [SerializeField] private float idleDuration;              // Dur�e de pause en idle, en secondes
    private float idleTimer = 0f;                             // Timer pour la dur�e en idle

    private bool ThisPNJDontWalk = false;

    void Start()
    {
        // Positionne le PNJ au premier point de destination
        if (tabPointDestination.Length > 0)
        {
            //On place corectement le PNJ, sur le premier point
            transform.position = tabPointDestination[0].transform.position;
            idleDuration = UnityEngine.Random.Range(5, 12);   // Dur�e al�atoire de pause
            vitesse = UnityEngine.Random.Range(1.5f, 2.5f);   // Vitesse al�atoire
            SetWalkingState(true);
        }
        else
        {
            animator.SetBool("isWalking", false); // Arr�te l'animation de marche si aucun point n'est d�fini
            ThisPNJDontWalk = true;
        }
    }

    void Update()
    {
        //Si quelqu'un veut faire pas bouger un PNJ, alors il a juste a ne mettre qu'un seul point
        if(ThisPNJDontWalk) return;

        if (isWalking)
        {
            MoveToNextDestination();
        }
        else
        {
            // Compte � rebours pendant l'idle
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                // Fin de l'idle, passage en mode marche
                idleTimer = 0f;
                currentDestinationIndex = (currentDestinationIndex + 1) % tabPointDestination.Length; // Changer de point de destination
                SetWalkingState(true);
            }
        }
    }

    private void MoveToNextDestination()
    {
        // Destination actuelle
        Transform targetPoint = tabPointDestination[currentDestinationIndex].transform;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, vitesse * Time.deltaTime);

        // V�rifie si la destination est atteinte
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            SetWalkingState(false); // Arr�t et passage en idle
        }
    }

    private void UpdateSpriteDirection()
    {
        // Tourne le sprite selon la direction du d�placement
        Transform targetPoint = tabPointDestination[currentDestinationIndex].transform;
        if (targetPoint.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    //G�re l��tat du PNJ (marche ou idle) et met � jour l�animation et la direction du sprite en cons�quence.
    private void SetWalkingState(bool walking)
    {
        isWalking = walking;
        animator.SetBool("isWalking", walking);

        if (walking)
        {
            UpdateSpriteDirection(); // Met � jour la direction au d�but de la marche
        }
    }
}
