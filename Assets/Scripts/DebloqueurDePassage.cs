using UnityEngine;
using static PathManager;

public class DebloqueurDePassage : MonoBehaviour
{
    public GameObject passage1;
    public GameObject passage2;
    public GameObject passage3;

    // Update is called once per frame
    void Update()
    {
        switch (CurrentPathState)
        {
            case PathState.Village1:
                break;
            case PathState.Mine:
                passage1.SetActive(false);
                break;
            case PathState.Village2:
                passage1.SetActive(false);
                break;
            case PathState.AssemblyFactory:
                passage1.SetActive(false);
                passage2.SetActive(false);
                break;
            case PathState.Village3:
                passage1.SetActive(false);
                passage2.SetActive(false);
                break;
            case PathState.RecycleFactory:
                passage1.SetActive(false);
                passage2.SetActive(false);
                passage3.SetActive(false);
                break;
            case PathState.Village4:
                passage1.SetActive(false);
                passage2.SetActive(false);
                passage3.SetActive(false);
                break;
            case PathState.End:
                passage1.SetActive(false);
                passage2.SetActive(false);
                passage3.SetActive(false);
                break;
            default:
                Debug.Log("Path state not found");
                break;
        }

    }
}
