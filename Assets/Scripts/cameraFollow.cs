using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    // Référence au transform du joueur
    public Transform playerTransform;
    
    // Vitesse de suivi de la caméra
    public float smoothSpeed = 5f;
    
    // Décalage de la caméra par rapport au joueur
    public Vector3 offset = new Vector3(0, 2, -10);

    // Limites de la carte  
    private GameObject GoMinBound;
    private GameObject GoMaxBound;
    private Vector2 minBounds; // Limite inférieure (gauche, bas)  
    private Vector2 maxBounds; // Limite supérieure (droite, haut)

    //Ces deux variable sont pour prendre en compte la taille de la camera
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void LateUpdate()
    {
        if (playerTransform == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

                if (GameObject.FindGameObjectWithTag("MinBound") != null)
                    GoMinBound = GameObject.FindGameObjectWithTag("MinBound");
                if (GameObject.FindGameObjectWithTag("MaxBound") != null)
                    GoMaxBound = GameObject.FindGameObjectWithTag("MaxBound");
                if(GoMinBound != null && GoMaxBound != null)
                {
                    minBounds = GoMinBound.transform.position;
                    maxBounds = GoMaxBound.transform.position;

                    cameraHalfHeight = Camera.main.orthographicSize;
                    cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

                    minBounds.x += cameraHalfWidth;
                    minBounds.y += cameraHalfHeight;
                    maxBounds.x -= cameraHalfWidth;
                    maxBounds.y -= cameraHalfHeight;
                }
                
            }
            else
            {
                playerTransform = null;
                gameObject.transform.position = new Vector3(0, 0, -10);
            }
        }
        else
        {
            // Calcul de la position cible
            Vector3 desiredPosition = playerTransform.position + offset;

            if (GoMinBound != null && GoMaxBound != null)
            {
                // Clamp la position désirée entre les limites
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
                desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
            }

            // Interpolation linéaire pour un mouvement fluide
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Application de la nouvelle position
            transform.position = smoothedPosition;
        }
    }
}
