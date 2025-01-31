using UnityEngine;
using static PathManager;

public class DebloqueurDePassage : MonoBehaviour
{
    public PathState pathToActivate;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDesactivate;

    private void Awake()
    {
        InputManager.OnPathStateChanged += UnlockPassage;
    }

    // Update is called once per frame
    void UnlockPassage()
    {
        if (CurrentPathState == pathToActivate)
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
