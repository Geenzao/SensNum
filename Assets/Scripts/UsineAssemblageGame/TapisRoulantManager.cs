using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


//Cette classe permet d'instenti� � la chaine des tapisroulant et
//de les faire avancer au rythme d�fini par le UsineAssemblageManager pour la vitesse des circuitImprimer

//Cette classe est un singleton

public class TapisRoulantManager : Singleton<TapisRoulantManager>
{
    [Header("TapisRoulant")]
    public GameObject tapisRoulantPrefab;

    [Header("Position")] 
    public GameObject start;
    public GameObject end;
    private Vector3 positionStart;
    private Vector3 positionEnd;


    private Vector3 endPosTapis; // Position de fin du tapis actuel
    private Vector3 beginPosTapis; // Position de debut du tapis actuel


    private List<GameObject> tapisRoulantList = new List<GameObject>();

    public float speed = 1;

    private int maxTapisRoulants = 3;

    //Au starte, on fait apparaitre trois tapis roulant
    void Start()
    {
        speed = UsineAssemblageGameManager.Instance.GetActualSpeed();

        positionEnd = end.transform.position;
        positionStart = start.transform.position;

        //On fait apparaitre les trois premiers tapis roulants

        GameObject FirsttapisRoulant = Instantiate(tapisRoulantPrefab, positionStart, Quaternion.identity);
        //FirsttapisRoulant.transform.SetParent(transform); //On les met en enfant de l'objet TapisRoulantManager
        tapisRoulantList.Add(FirsttapisRoulant);

        // Cr�er deux autres tapis roulants initiaux
        for (int i = 0; i < 2; i++)
        {
            CreateTapisRoulant();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Faire avancer les tapis roulants et v�rifier s'ils doivent �tre d�truits
        for (int i = tapisRoulantList.Count - 1; i >= 0; i--)
        {
            GameObject tapisRoulant = tapisRoulantList[i];
            tapisRoulant.transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

            // Si le tapis sort de l'�cran (ou atteint positionEnd), on le d�truit
            // Si le tapis roulant d�passe positionEnd, on le d�truit
            if (tapisRoulant.transform.Find("begin").position.x > positionEnd.x)
            {
                Destroy(tapisRoulant);
                tapisRoulantList.RemoveAt(i);
                CreateTapisRoulant();
            }
        }
    }

    // Cr�er un nouveau tapis roulant � une position donn�e
    private void CreateTapisRoulant()
    {
        print("CreateTapisRoulant");
        GameObject NewtapisRoulant = Instantiate(tapisRoulantPrefab, Vector3.zero, Quaternion.identity);

        //On place le nouveau tapis � gauche du dernier tapis roulant
        GameObject dernierTapis = tapisRoulantList[tapisRoulantList.Count - 1];
        Vector3 posBeginDernier = dernierTapis.transform.Find("begin").position;
        NewtapisRoulant.transform.position = posBeginDernier;
        Vector3 posEndActual = NewtapisRoulant.transform.Find("end").position;

        NewtapisRoulant.transform.position += posBeginDernier - posEndActual;

        //NewtapisRoulant.transform.SetParent(transform);
        tapisRoulantList.Add(NewtapisRoulant);
    }

    // Modifier la vitesse
    public void AddSpeed(float additionalSpeed)
    {
        speed += additionalSpeed;
    }
}
