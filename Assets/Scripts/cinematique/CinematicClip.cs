using UnityEngine;
using System.Collections.Generic;

/*
 * Cette classe repr�sente un clip d'une cinematque qui est une suite de clip. 
 * 
 * Elle g�re la vitesse de d�placement des images qui sont tri� dans diff�rent Layer
 * 
 * Chaque Layer repr�sente une couche qui ira un peu moin vite que la suivante pour donner l'ilusion du mouvement
 */


public class CinematicClip : MonoBehaviour
{
    [System.Serializable]
    public struct ParallaxLayer
    {
        [Header("Tableau des elems par couches")]
        public GameObject[] tabImages; // Tableau pour stocker les GameObjects dans un layer
    }

    [Header("Tableau des diff�rents Couches")]
    public ParallaxLayer[] tabLayer = new ParallaxLayer[0]; // Tableau de ParallaxLayer pour stoker les images et les couches

    [Header("Variable")]
    public float baseSpeed; // Vitesse de base pour le d�placement
    public float stepSpeed; // Vitesse de base pour le d�placement
    public Vector3 direction; // Direction du mouvement (par d�faut vers la gauche)


    [SerializeField] private float[] tabSpeedLayer;

    public bool isRening = false;

    void Start()
    {

        //pour chaque couche, on incr�mente la vitesse du pas
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
                    // D�place l'image dans la direction donn�e � la vitesse du layer auquelle elle appartient
                    image.transform.Translate(direction * tabSpeedLayer[i] * Time.deltaTime);
                }
            }
        }

    }
}
