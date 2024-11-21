using UnityEngine;
using System.Collections.Generic;

public class SpawnAndDropManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Vector2 zoneSize;
    [SerializeField] private float spawnInterval = 1f;

    public GameObject btnVert;
    private bool gameStarted = false; // Variable pour suivre l'état du jeu

    void Start()
    {
        // Ne rien faire au démarrage
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Clickable"))
            {
                if (btnVert != null)
                {
                    btnVert.SetActive(false); // Désactiver l'objet cible
                    StartGame(); // Démarrer le jeu
                }
            }
        }
    }

    void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            InvokeRepeating("SpawnObject", 0f, spawnInterval); // Démarrer le spawn des objets
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
        movable.speed = 2f; // Ajoute le script de mouvement et définit la vitesse
        instantiated.AddComponent<CanvasGroup>(); // Nécessaire pour le drag and drop

        // Ajoutez un composant Image pour permettre le raycast
        instantiated.AddComponent<UnityEngine.UI.Image>().raycastTarget = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}