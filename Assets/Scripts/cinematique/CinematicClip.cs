using UnityEngine;
using System.Collections.Generic;

public class CinematicClip : MonoBehaviour
{
    [System.Serializable]
    public struct ParallaxLayer
    {
        [Header("Tableau des �l�ments par couches")]
        public GameObject[] tabImages; // Tableau pour stocker les GameObjects dans un layer
    }

    [Header("Tableau des diff�rents Couches")]
    public ParallaxLayer[] tabLayer = new ParallaxLayer[0]; // Tableau de ParallaxLayer pour stocker les images et les couches

    [Header("Variables de vitesse")]
    public float baseSpeed; // Vitesse de base pour le d�placement
    public float stepSpeed; // Incr�mentation de la vitesse pour chaque couche
    private Vector3 direction = Vector3.left; // Direction du mouvement (par d�faut vers la gauche)

    [SerializeField] private float[] tabSpeedLayer;

    [Header("Gestion du temps")]
    public float resetTime = 10f; // Temps avant de r�initialiser (en secondes)
    private float elapsedTime = 0f; // Temps �coul� depuis le d�but du mouvement

    public bool isRening = false; // pour que le Cinematic manager puisse d�cid� quelle cinematicClip s'actionne

    // Stockage des positions initiales
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        // Initialiser les vitesses pour chaque couche
        tabSpeedLayer = new float[tabLayer.Length];
        tabSpeedLayer[0] = baseSpeed;
        for (int i = 1; i < tabSpeedLayer.Length; i++)
        {
            tabSpeedLayer[i] = tabSpeedLayer[i - 1] + stepSpeed;
        }

        // Stocker les positions initiales de toutes les images
        foreach (var layer in tabLayer)
        {
            foreach (var image in layer.tabImages)
            {
                if (image != null && !initialPositions.ContainsKey(image))
                {
                    initialPositions[image] = image.transform.position;
                }
            }
        }
    }

    void Update()
    {
        if (isRening)
        {
            MoveLayer();
            elapsedTime += Time.deltaTime;

            // R�initialisation apr�s le temps d�fini
            if (elapsedTime >= resetTime)
            {
                ResetToInitialState();
            }
        }
    }

    private void MoveLayer()
    {
        if (!isRening) return;

        // Parcours chaque couche
        for (int i = 0; i < tabLayer.Length; i++)
        {
            // Parcours chaque image dans la couche
            foreach (GameObject image in tabLayer[i].tabImages)
            {
                if (image != null)
                {
                    // D�place l'image dans la direction donn�e � la vitesse du layer auquel elle appartient
                    image.transform.Translate(direction * tabSpeedLayer[i] * Time.deltaTime);
                }
            }
        }
    }

    private void ResetToInitialState()
    {
        Debug.Log("R�initialisation des positions initiales.");

        // Remettre toutes les images � leurs positions initiales
        foreach (var kvp in initialPositions)
        {
            if (kvp.Key != null)
            {
                kvp.Key.transform.position = kvp.Value;
            }
        }

        // R�initialiser le minuteur
        elapsedTime = 0f;
    }
}
