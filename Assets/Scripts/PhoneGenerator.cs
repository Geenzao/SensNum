using UnityEngine;
using System.Collections.Generic;
using System.Threading;
//Ce scripte permet de cr�� des phone et de les d�plac� sur le tapis roulant puis de les d�truire



public class PhoneGenerator : MonoBehaviour
{
    public GameObject phonePrefab; // Prefab du t�l�phone � g�n�rer
    public Transform spawnPoint;   // Point de g�n�ration du t�l�phone
    public Transform endPoint;     // Point de fin du t�l�phone

    public float speed = 1f;        // Vitesse de d�placement des t�l�phones
    public float time = 1f;

    private float spawnTimer = 0f;  // Timer pour la g�n�ration des t�l�phones
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

        //Faire bouger les t�l�phone
        for(int i =0; i < lstphones.Count; i++)
        {
            lstphones[i].transform.position = Vector3.MoveTowards(lstphones[i].transform.position, endPoint.position, speed * Time.deltaTime);

            //Si le t�l�phone arrive � la fin, on le d�truit
            if (lstphones[i].transform.position == endPoint.position)
            {
                Destroy(lstphones[i]);
                lstphones.RemoveAt(i);
            }
        }
    }

    //Fonction pour cr�� les t�l�phones
    private void CreateNewPhone()
    {
        GameObject phone = Instantiate(phonePrefab, spawnPoint.position, Quaternion.identity);
        lstphones.Add(phone);
    }

}
