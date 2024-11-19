using UnityEngine;
using System.Collections.Generic;

/*
 * Cette classe représente un clip d'une cinematque qui est une suite de clip. 
 * 
 * Elle gère la vitesse de déplacement des images qui sont trié dans différent Layer
 * 
 * Chaque Layer représente une couche qui ira un peu moin vite que la suivante pour donner l'ilusion du mouvement
 */


public class CinematicClip : MonoBehaviour
{
    [System.Serializable]
    public struct ParallaxLayer
    {
        [Header("Tableau des elems par couches")]
        public GameObject[] tabImages; // Tableau pour stocker les GameObjects dans un layer
    }

    [Header("Tableau des différents Couches")]
    public ParallaxLayer[] tabLayer = new ParallaxLayer[0]; // Tableau de ParallaxLayer pour stoker les images et les couches

    [Header("Variable")]
    public float baseSpeed; // Vitesse de base pour le déplacement
    public float stepSpeed; // Vitesse de base pour le déplacement
    public Vector3 direction; // Direction du mouvement (par défaut vers la gauche)


    [SerializeField] private float[] tabSpeedLayer;

    public bool isRening = false;

    void Start()
    {

        //pour chaque couche, on incrémente la vitesse du pas
        tabSpeedLayer = new float[tabLayer.Length];

        tabSpeedLayer[0] = baseSpeed;
        for (int i = 1; i < tabSpeedLayer.Length; i++)
        {
            tabSpeedLayer[i] = tabSpeedLayer[i-1] + stepSpeed;
        }
    }

    void Update()
    {
        MoveLayer();
    }

    private void MoveLayer()
    {
        if (isRening == false) return;
        // Parcours chaque couche
        for (int i = 0; i < tabLayer.Length; i++)
        {
            // Parcours chaque image dans la couche
            foreach (GameObject image in tabLayer[i].tabImages)
            {
                if (image != null)
                {
                    // Déplace l'image dans la direction donnée à la vitesse du layer auquelle elle appartient
                    image.transform.Translate(direction * tabSpeedLayer[i] * Time.deltaTime);
                }
            }
        }

    }
}
