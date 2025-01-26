using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


//Cette classe permet d'instenti� � la chaine des tapisroulant et
//de les faire avancer au rythme d�fini par le UsineAssemblageManager pour la vitesse des circuitImprimer

//Cette classe est un singleton

public class TapisRoulantManager : Singleton<TapisRoulantManager>
{
    [Header("TapisRoulant")] GameObject tapisRoulantPrefab;

    [Header("Position")] 
    public GameObject start;
    public GameObject end;
    public GameObject interval; //quand un tapis roulant d�passe ce point, un autre apparait
    private Vector3 positionStart;
    private Vector3 positionEnd;
    private Vector3 positionInterval; //quand un tapis roulant d�passe ce point, un autre apparait

    private List<GameObject> tapisRoulantList = new List<GameObject>();

    public float speed = 1;

    //Au starte, on fait apparaitre trois tapis roulant
    void Start()
    {
        speed = UsineAssemblageGameManager.Instance.GetActualSpeed();

        positionEnd = end.transform.position;
        positionStart = start.transform.position;
        positionInterval = interval.transform.position;

        // Cr�er trois tapis roulants initiaux
        for (int i = 0; i < 3; i++)
        {
            CreateTapisRoulant(i == 0 ? positionStart : GetNextPosition());
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
            if (tapisRoulant.transform.position.x > positionInterval.x)
            {
                Destroy(tapisRoulant);
                tapisRoulantList.RemoveAt(i);
                CreateTapisRoulant(GetNextPosition());
            }
        }
    }

    // Cr�er un nouveau tapis roulant � une position donn�e
    private void CreateTapisRoulant(Vector3 position)
    {
        GameObject tapisRoulant = Instantiate(tapisRoulantPrefab, position, Quaternion.identity);
        tapisRoulant.transform.SetParent(transform);
        tapisRoulantList.Add(tapisRoulant);
    }

    // Calculer la position du prochain tapis roulant
    private Vector3 GetNextPosition()
    {
        if (tapisRoulantList.Count == 0)
        {
            return positionStart;
        }

        // On prend le dernier tapis roulant de la liste
        GameObject dernierTapis = tapisRoulantList[tapisRoulantList.Count - 1];

        // Pour obtenir le point end de ce tapis
        Transform endPoint = dernierTapis.transform.Find("end");

        if (endPoint == null)
        {
            Debug.LogError("Le prefab du tapis roulant doit contenir un enfant nomm� 'end'.");
            return positionStart;
        }

        return endPoint.position - ((dernierTapis.transform.Find("begin").position - endPoint.position)/2);
    }

    // Modifier la vitesse
    public void AddSpeed(float additionalSpeed)
    {
        speed += additionalSpeed;
    }
}
