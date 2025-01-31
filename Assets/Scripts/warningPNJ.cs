using UnityEngine;

public class warningPNJ : MonoBehaviour
{
    public GameObject warning;

    public Animator animator;

    void Start()
    {
        animator.SetTrigger("go");
    }

    public void hideWarning()
    {
        warning.SetActive(false);
    }
}
