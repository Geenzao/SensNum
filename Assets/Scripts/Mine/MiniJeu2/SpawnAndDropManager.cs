using UnityEngine;
using System.Collections.Generic;

public class SpawnAndDropManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Vector2 zoneSize;
    [SerializeField] private float spawnInterval = 1f;

    private bool gameStarted = false; // Variable pour suivre l'�tat du jeu

    // Probabilit�s d'apparition bas�es sur la teneur
    private Dictionary<int, float> spawnChances = new Dictionary<int, float>
    {
        { 0, 0.01f },       // Or (0,0002 %)    --> 1%
        { 1, 0.03f },       // Cuivre (0,2 %)   --> 3%
        { 2, 0.08f }        // Lithium (1 %)    --> 8%
    };

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
        GameObject selectedObject = GetRandomObject();

        Vector2 spawnPosition = new Vector2(
            Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
            Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2)
        );

        GameObject instantiated = Instantiate(selectedObject, spawnPosition, Quaternion.identity);
        instantiated.AddComponent<CanvasGroup>(); // N�cessaire pour le drag and drop
        instantiated.AddComponent<UnityEngine.UI.Image>().raycastTarget = true;
    }

    GameObject GetRandomObject()
    {
        float randomValue = Random.value; // Nombre entre 0 et 1

        // V�rifier les objets sp�ciaux (or, cuivre, lithium)
        foreach (var entry in spawnChances)
        {
            if (randomValue < entry.Value)
            {
                return objectsToSpawn[entry.Key];
            }
        }

        // Si aucun des objets sp�ciaux n'a �t� s�lectionn�, prendre un autre au hasard
        int randomIndex = Random.Range(3, objectsToSpawn.Length);
        return objectsToSpawn[randomIndex];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }
}
