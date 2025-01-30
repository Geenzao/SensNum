using UnityEngine;
using System.Collections.Generic;

public class CameraCredits : MonoBehaviour
{
    public List<Transform> points; // Liste des points de passage
    public float duration = 5.0f; // Durée du déplacement entre chaque point en secondes
    private int currentPointIndex = 0;
    private float elapsedTime = 0.0f;

    // Start est appelé une fois avant la première exécution de Update après la création du MonoBehaviour
    void Start()
    {
        if (points.Count > 0)
        {
            transform.position = new Vector3(points[0].position.x, points[0].position.y, transform.position.z);
        }
    }

    // Update est appelé une fois par frame
    void Update()
    {
        if (points.Count > 1 && currentPointIndex < points.Count - 1)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Vector3 startPosition = points[currentPointIndex].position;
            Vector3 endPosition = points[currentPointIndex + 1].position;
            transform.position = new Vector3(
                Mathf.Lerp(startPosition.x, endPosition.x, t),
                Mathf.Lerp(startPosition.y, endPosition.y, t),
                transform.position.z
            );

            if (t >= 1.0f)
            {
                elapsedTime = 0.0f;
                currentPointIndex++;
            }
        }
    }
}