using UnityEngine;

public class MoveTruck : MonoBehaviour
{
    // Variables priv�es pour la d�tection de collision et la vitesse
    private float collisionRadius = 0.5f;
    private float speed = 0f;
    public Vector3 initialPosition;
    private bool isMoving = false;
    private bool hasTriggered = false;
    public ThirdMiniGame thirdMiniGame;

    // M�thode appel�e une fois au d�but
    void Start()
    {
        // Initialisation de la vitesse al�atoire et de la position initiale
        speed = Random.Range(2f, 4f);
        initialPosition = transform.position;
    }

    // M�thode appel�e � chaque frame
    void Update()
    {
        if (isMoving)
        {
            // D�placement du camion
            Vector3 movement = transform.right * -1 * speed * Time.deltaTime;
            transform.position += movement;

            // D�tection des collisions avec d'autres camions
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, collisionRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != gameObject && (hitCollider.CompareTag("Truck") || hitCollider.CompareTag("TruckOre")))
                {
                    isMoving = false;
                    transform.position = initialPosition;
                    break;
                }
            }

            // D�tection des collisions avec la zone limite
            Collider2D[] limitColliders = Physics2D.OverlapCircleAll(transform.position, collisionRadius);
            foreach (var limitCollider in limitColliders)
            {
                if (!hasTriggered && limitCollider.gameObject != gameObject && limitCollider.CompareTag("LimitZone"))
                {
                    hasTriggered = true;
                    if (CompareTag("TruckOre"))
                    {
                        thirdMiniGame.IncrementTruckOreCounter();
                    }
                    else
                    {
                        thirdMiniGame.IncrementTruckCounter();
                    }
                    // plutot que destroy on stop le gameobject et on le set visible false
                    isMoving = false;
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    // M�thode appel�e lors du clic de la souris sur l'objet
    void OnMouseDown()
    {
        if (CompareTag("Truck") || CompareTag("TruckOre"))
        {
            isMoving = true;
            /*AudioManager.Instance.PlaySoundEffet(AudioType.Camion);*/
        }
    }

    // M�thode pour dessiner des gizmos dans l'�diteur Unity
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }

    // M�thode pour r�initialiser le camion
    public void ResetTruck()
    {
        transform.position = initialPosition;
        isMoving = false;
        hasTriggered = false;
        gameObject.SetActive(true);
    }
}