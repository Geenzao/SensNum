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
    public bool isValid = false;
    
    void Start()
    {
        this.speed = UsineAssemblageGameManager.Instance.GetActualSpeed();
        this.endPositionX = UsineAssemblageGameManager.Instance.GetEndPosition();
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
        else Debug.LogWarning("Mauvais type");
    }


    private void MoveCircuit()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > endPositionX)
        {
            isValid = CheckValidity();
            //On signal au UsineAssemblageGameManager si le joueur a réussi le circuit
            //Et on détruit le circuit 
            if(CheckValidity())
                UsineAssemblageGameManager.Instance.AddGoodCircuit();
            else
                UsineAssemblageGameManager.Instance.AddBadCircuit();

            DestroyThis();
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
}