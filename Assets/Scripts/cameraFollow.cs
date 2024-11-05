using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    // Référence au transform du joueur
    public Transform playerTransform;
    
    // Vitesse de suivi de la caméra
    public float smoothSpeed = 5f;
    
    // Décalage de la caméra par rapport au joueur
    public Vector3 offset = new Vector3(0, 2, -10);

    void LateUpdate()
    {
        if (playerTransform == null)
            return;

        // Calcul de la position cible
        Vector3 desiredPosition = playerTransform.position + offset;
        
        // Interpolation linéaire pour un mouvement fluide
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Application de la nouvelle position
        transform.position = smoothedPosition;
    }
}
