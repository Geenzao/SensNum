using UnityEngine;


//Pour que les porte de l'usine d'assemblage s'ouvre toute seul quand le joeur arrive assez proche

public class OpenDoor : MonoBehaviour
{
    public Animator anim;


    private void Start()
    {
        anim.SetTrigger("open");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("close");
        }
    }
}
