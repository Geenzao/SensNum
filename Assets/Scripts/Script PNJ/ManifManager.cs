using UnityEngine;

public class ManifManager : Singleton<ManifManager>
{
    [Header("PNJ manifestant")]
    public MouvementPNJ[] tabPNJ;

    public GameObject ptsDestination;
    public GameObject ptsDepart;


    void Start()
    {
        //On passe tous les PNJ à manifestant
        foreach (MouvementPNJ pnj in tabPNJ)
        {
            pnj.SetStateManif(ptsDestination.transform, ptsDepart.transform);
        }
    }


    void Update()
    {
        
    }
}
