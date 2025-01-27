using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


//Cette classe représente un endroit dans le circuit qui attend un composant 
//C'est en quelque sorte une place libre dans le circuit que le joueur doit remplir avec un composant
[System.Serializable]
public class ComponentPlace
{
    public GameObject component; // Composant à la bonne position et avec le bon visuel qui est caché au début 
    public ComponentType typeAccepted;         // Type de composant attendu (ex. red, gray, etc.)
    public bool isFill = false;
}

public class CircuitImprime : MonoBehaviour
{
    public List<ComponentPlace> lstComponentPlaceOnCircuit;  // Liste des composants et leurs positions sur le circuit

    private float speed = 0;
    private float endPositionX;
    private float positionSpawnOther;//Si le circuit atteint ce point, il dit au manager de faire apparaît un autre circuit
    public bool isValid = false;
    private bool isAbleToSpawnOtherCircuit = true; //un circuit ne peut faire spawn un autre qu'une seul fois

    void Start()
    {
        this.speed = UsineAssemblageGameManager.Instance.GetActualSpeed();
        this.endPositionX = UsineAssemblageGameManager.Instance.GetEndPosition();
        this.positionSpawnOther = UsineAssemblageGameManager.Instance.GetSpawnInterval();
    }

    void Update()
    {
        MoveCircuit();
    }

    //Cette fct sert à placé un composant sur une des place
    public void FillComposantPlace(ComponentType typeNewComponent, int indexComponentPlace)
    {
        if (lstComponentPlaceOnCircuit[indexComponentPlace].typeAccepted == typeNewComponent)
        {
            this.lstComponentPlaceOnCircuit[indexComponentPlace].isFill = true;
            this.lstComponentPlaceOnCircuit[indexComponentPlace].component.SetActive(true);
        }
        //else Debug.LogWarning("Mauvais type");

        if(CheckValidity())
        {
            UsineAssemblageGameManager.Instance.AddGoodCircuit();
        }
    }

    private void MoveCircuit()
    {
        // Translate vers la droite en ignorant la rotation locale de l'objet
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

        if (transform.position.x > endPositionX)
        {
            DestroyThis();
        }

        //SI on a atteint un point précis, on dit au manager de faire apparaitre un autre circuit
        if (Mathf.Abs(transform.position.x - positionSpawnOther) <= 0.04 && isAbleToSpawnOtherCircuit)
        {
            //print("SPAWN OTHER CIRCUIT");
            UsineAssemblageGameManager.Instance.SpawnCircuitImprime();
            isAbleToSpawnOtherCircuit = false;
        }
    }

    private bool CheckValidity()
    {
        foreach (var compPlace in lstComponentPlaceOnCircuit)
        {
            // Si un emplacement est vide ou a le mauvais composant, retourne faux
            if (compPlace.isFill == false)
            {
                return false;
            }
        }
        return true; // Si tous les composants sont correctement placés, retourne vrai
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void addSpeed(float s)
    {
        speed += s;
    }
}