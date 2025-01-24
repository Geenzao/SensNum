using UnityEngine;
using System.Collections;
public class chat_mouve : MonoBehaviour
{

    public Animator anim;
    public int number;
    public int numberofanimation;

    void Start()
    {
        StartCoroutine(GenerateRandomNumber());
    }

    IEnumerator GenerateRandomNumber()
    {
        while (true)
        {
            number = Random.Range(1, numberofanimation+1); // Génère un nombre entre 1 et 3
            

            
            anim.SetTrigger("anim" + number);
            
            
            

            yield return new WaitForSeconds(5); // Attend 5 secondes
            
            
        }
    }
}
