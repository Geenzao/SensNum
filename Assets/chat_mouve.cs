using UnityEngine;
using System.Collections;
public class chat_mouve : MonoBehaviour
{

    public Animator anim;
    public int number;
    public int numberofanimation;

    public bool TypeChatSound = true;

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

            if(TypeChatSound == true)
            {
                int numberAudio = Random.Range(1, 3); // Génère un nombre entre 1 et 2
                AudioClip clip;
                if (numberAudio == 1) 
                {
                    clip = AudioManager.Instance.GetClip(AudioType.Chat2);
                }
                else
                clip = AudioManager.Instance.GetClip(AudioType.Chat1);

                AudioSource hh = GetComponent<AudioSource>();

                hh.clip = clip;
                hh.Play();
            }



            yield return new WaitForSeconds(5); // Attend 5 secondes
            
            
        }
    }
}
