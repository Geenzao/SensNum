using UnityEngine;
using System.Collections.Generic;

public class SpawnAndDropManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Vector2 zoneSize;
    [SerializeField] private float spawnInterval = 1f;

    private bool gameStarted = false; // Variable pour suivre l'�tat du jeu

    void Start()
    {
        // Ne rien faire au d�marrage
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            InvokeRepeating("SpawnObject", 0f, spawnInterval); // D�marrer le spawn des objets
        }
    }

    void SpawnObject()
    {
        GameObject selectedObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        Vector2 spawnPosition = new Vector2(
            Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
            Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2)
        );

        GameObject instantiated = Instantiate(selectedObject, spawnPosition, Quaternion.identity);
        MovableObject movable = instantiated.AddComponent<MovableObject>();
        movable.speed = 2f; // Ajoute le script de mouvement et d�finit la vitesse
        instantiated.AddComponent<CanvasGroup>(); // N�cessaire pour le drag and drop

        // Ajoutez un composant Image pour permettre le raycast
        instantiated.AddComponent<UnityEngine.UI.Image>().raycastTarget = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}
