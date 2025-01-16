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
        }

    }
}
