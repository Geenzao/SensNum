using UnityEngine;

public class warningPNJ : MonoBehaviour
{
    public GameObject warning;

    public Animator animator;
    public bool NEPASAFFICHER = false;

    void Start()
    {
        if(NEPASAFFICHER)
        {
            warning.SetActive(false);
        }
        animator.SetTrigger("go");
    }

    public void hideWarning()
    {
        warning.SetActive(false);
    }
}
