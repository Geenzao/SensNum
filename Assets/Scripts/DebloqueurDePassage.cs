using UnityEngine;
using static PathManager;

public class DebloqueurDePassage : MonoBehaviour
{
    public PathManager.PathState pathToActivate;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDesactivate;

    // Update is called once per frame
    void Update()
    {
        if(PathManager.CurrentPathState == pathToActivate)
        {
            for(int i = 0; i < objectsToActivate.Length; i++)
            {
                objectsToActivate[i].SetActive(true);
            }
            for (int i = 0; i < objectsToDesactivate.Length; i++)
            {
                objectsToDesactivate[i].SetActive(false);
            }
        }
    }
}
