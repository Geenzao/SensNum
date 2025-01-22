using UnityEngine;
using System.Collections.Generic;
using System.Threading;
//Ce scripte permet de créé des phone et de les déplacé sur le tapis roulant puis de les détruire



public class PhoneGenerator : MonoBehaviour
{
    public GameObject phonePrefab; // Prefab du téléphone à générer
    public Transform spawnPoint;   // Point de génération du téléphone
    public Transform endPoint;     // Point de fin du téléphone

    public float speed = 1f;        // Vitesse de déplacement des téléphones
    public float time = 1f;

    private float spawnTimer = 0f;  // Timer pour la génération des téléphones
    private float spawnTime = 0f;
    private int spawnCount = 0;

    private List<GameObject> lstphones = new List<GameObject>();

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0f;
            CreateNewPhone();
        }

        //Faire bouger les téléphone
        for(int i =0; i < lstphones.Count; i++)
        {
            lstphones[i].transform.position = Vector3.MoveTowards(lstphones[i].transform.position, endPoint.position, speed * Time.deltaTime);

            //Si le téléphone arrive à la fin, on le détruit
            if (lstphones[i].transform.position == endPoint.position)
            {
                Destroy(lstphones[i]);
                lstphones.RemoveAt(i);
            }
        }
    }

    //Fonction pour créé les téléphones
    private void CreateNewPhone()
    {
        GameObject phone = Instantiate(phonePrefab, spawnPoint.position, Quaternion.identity);
        lstphones.Add(phone);
    }

}
