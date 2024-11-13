using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ComposantPlace
{
    public GameObject composantPlace; // Position spécifique pour placer le composant
    public ComposantType type;         // Type de composant attendu (ex. red, gray, etc.)
    public bool isFill = false;
    public GameObject composantUser = null; //composant placé sur le circuit à la place

}

public class CircuitImprime : MonoBehaviour
{
    public List<ComposantPlace> composantOnCircuit;  // Liste des composants et leurs positions sur le circuit
    public List<GameObject> composantUser; //Liste de composant que le User a choisie de place sur ce circuit

    private float speed = 0;
    private float endPositionX;
    public bool IWantToDie = false;
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

    private void MoveCircuit()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > endPositionX)
        {
            isValid = CheckValidity();
            //On signal au UsineAssemblageGameManager de nous détruire
            //IWantToDie = true;
            if(CheckValidity())
            {
                UsineAssemblageGameManager.Instance.AddGoodCircuit();
            }
            else
                UsineAssemblageGameManager.Instance.AddBadCircuit();
            Destroy(gameObject);
        }
    }

    private bool CheckValidity()
    {
        foreach (var compPlace in composantOnCircuit)
        {
            // Si un emplacement est vide ou a le mauvais composant, retourne faux
            if (compPlace.isFill == false 
                || compPlace.composantUser.GetComponent<Composant>().type.Equals(compPlace.type) == false )
            {
                return false;  
            }
        }
        return true; // Si tous les composants sont correctement placés, retourne vrai
    }
}