using UnityEngine;

public class mouseFollow : MonoBehaviour
{
    // D finition d'une variable pour la cam ra principale
    private Camera mainCamera;

    void Start()
    {
        // R cup ration de la cam ra principale
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            // R cup ration de la position de la souris dans l espace  cran
            Vector3 mousePosition = Input.mousePosition;

            // Conversion de la position de la souris en coordonn es monde
            mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

            // Mise   jour de la position de l'objet pour suivre la souris
            // On garde la position z d'origine pour  viter que l'objet disparaisse
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }
}

