using System.Collections;
using UnityEngine;

// Ce script déplace les PNJ en suivant un parcours aller-retour entre deux points, avec des pauses obligatoires en mode idle.

public class MouvementPNJ : MonoBehaviour
{
    public GameObject[] tabPointDestination; // Deux points pour le déplacement en boucle
    public Animator animator;

    [SerializeField] private int currentDestinationIndex = 0; // Index du point de destination actuel
    [SerializeField] private bool isWalking = true;           // Contrôle si le PNJ marche ou est en idle
    [SerializeField] private float vitesse = 2f;              // Vitesse de déplacement
    [SerializeField] private float idleDuration;              // Durée de pause en idle, en secondes
    private float idleTimer = 0f;                             // Timer pour la durée en idle

    private bool ThisPNJDontWalk = false;

    void Start()
    {
        // Positionne le PNJ au premier point de destination
        if (tabPointDestination.Length > 0)
        {
            //On place corectement le PNJ, sur le premier point
            transform.position = tabPointDestination[0].transform.position;
            idleDuration = UnityEngine.Random.Range(5, 12);   // Durée aléatoire de pause
            vitesse = UnityEngine.Random.Range(1.5f, 2.5f);   // Vitesse aléatoire
            SetWalkingState(true);
        }
        else
        {
            animator.SetBool("isWalking", false); // Arrête l'animation de marche si aucun point n'est défini
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
            // Compte à rebours pendant l'idle
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

        // Vérifie si la destination est atteinte
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            SetWalkingState(false); // Arrêt et passage en idle
        }
    }

    private void UpdateSpriteDirection()
    {
        // Tourne le sprite selon la direction du déplacement
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

    //Gère l’état du PNJ (marche ou idle) et met à jour l’animation et la direction du sprite en conséquence.
    private void SetWalkingState(bool walking)
    {
        isWalking = walking;
        animator.SetBool("isWalking", walking);

        if (walking)
        {
            UpdateSpriteDirection(); // Met à jour la direction au début de la marche
        }
    }
}
