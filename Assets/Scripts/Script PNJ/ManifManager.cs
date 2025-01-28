using UnityEngine;
using System.Collections;

public class ManifManager : Singleton<ManifManager>
{
    [Header("PNJ manifestant")]
    public MouvementPNJ[] tabPNJ;

    public GameObject ptsDestination;
    public GameObject ptsDepart;

    public GameObject ptsHautMax;
    public GameObject ptsBasMax;

    private Vector3[] tabDestinationManif = new Vector3[2];
    private int indexDestinationManif = 0;

    private float MaxEcartToDestination = 2f; //pour que tous les PNJ ne s'arrête pas à la même ligne 
    private float MinEcartToDestination = 0.1f; //pour que tous les PNJ ne s'arrête pas à la même ligne
    private float ecart = 0.0f;

    private int nbPNJ;
    private int nbPNJArrived;

    private Coroutine couroutineRef = null;

    void Start()
    {
        tabDestinationManif[0] = ptsDepart.transform.position;
        tabDestinationManif[1] = ptsDestination.transform.position;
        GoManif();
    }

    //Il faut lancer cette fonction pour activer la manif
    public void GoManif()
    {
        float vitesseMax = 2.3f;
        float vitesseMin = 1.7f;

        foreach (MouvementPNJ pnj in tabPNJ)
        {
            if (pnj != null)
            {
                pnj.SetStateManif(ptsDestination.transform, ptsDepart.transform, Random.Range(vitesseMin, vitesseMax), GetEcartToDestination());
                pnj.changeToWlaking();
                nbPNJ++;

                //On place le PNJ sur la ligne vertical du start
                pnj.transform.position = new Vector3(GetXPosition(), GetYPosition(), pnj.transform.position.z);
            }
            else
            {
                Debug.LogWarning("Un PNJ dans tabPNJ est null !");
            }
        }
    }

    public void PNJArrived()
    {
        nbPNJArrived++;
        if (nbPNJArrived == nbPNJ)
        {
            Debug.Log("Tous les PNJ sont arrivés");
            if(couroutineRef != null)
                StopCoroutine(couroutineRef);
            couroutineRef = StartCoroutine(Attendre());
        }
    }

    private IEnumerator Attendre()
    {
        yield return new WaitForSeconds(5);
        AllPNJGoInvers();
    }

    private void AllPNJGoInvers()
    {
        foreach (MouvementPNJ pnj in tabPNJ)
        {
            pnj.ChangeDestinationManif();
        }

        // Réinitialise les compteurs
        nbPNJArrived = 0;

        Debug.Log("Les PNJ changent de direction !");
    }

    private float GetEcartToDestination() => Random.Range(MinEcartToDestination, MaxEcartToDestination);
    private float GetXPosition() => Random.Range(ptsDepart.transform.position.x, ptsDestination.transform.position.x);
    private float GetYPosition() => Random.Range(ptsBasMax.transform.position.y, ptsHautMax.transform.position.y);

}
