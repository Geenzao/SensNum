using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO : bloquer les mouvement du joueur quand le dialogue est lanc 


public class dialogueManager : MonoBehaviour
{
    public Text dialogueTextUI; //texte qui serra modifi  avec les phrases des PNJ
    public GameObject dialoguePanelUI; //Object UI du dialogue, ex : paneau gris ou appara t les phrase
    //public GameObject PanelUITextInteraction;

    private bool isDialogueActive = false;
    private Queue<string> qSentences;

    public static dialogueManager instance;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("Attention plusieur instance de DialogueManager");
            return;
        }
        instance = this;

        qSentences = new Queue<string>();
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(dialoguePNJ diag)
    {
        //PanelUITextInteraction.SetActive(false);
        isDialogueActive = true;
        qSentences.Clear();
        //On affiche l'UI du dialogue
        dialoguePanelUI.SetActive(true);

        int nbInteraction = diag.getInteractionCount();
        print("Dans le manager" + nbInteraction);
        if (nbInteraction < diag.tabDialogue.Length)
        {
            foreach (string sentence in diag.tabDialogue[nbInteraction].sentences)
            {
                qSentences.Enqueue(sentence);
            }
        }
        else
        {
            //ICI  a veut dire qu'il y a une erreur dans le nombre de dialogue.
            //En cas d'erreur, on repete le dernier dialogue du PNJ en boucle
            nbInteraction = diag.tabDialogue.Length - 1;
            foreach (string sentence in diag.tabDialogue[nbInteraction].sentences)
            {
                qSentences.Enqueue(sentence);
            }
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (qSentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = qSentences.Dequeue();
        StopAllCoroutines();//empecher affichage de plusieur phrase si appuy sur suivant avaant la fin de la premiere coroutine
        StartCoroutine(LettreParLettre(sentence));
    }

    IEnumerator LettreParLettre(string sentence)
    {
        dialogueTextUI.text = "";
        foreach (char lettre in sentence.ToCharArray())
        {
            dialogueTextUI.text += lettre;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void EndDialogue()
    {
        //PanelUITextInteraction.SetActive(true);
        dialoguePanelUI.SetActive(false);
        isDialogueActive = false;
    }

    public bool fctisDialogueActive()
    {
        return isDialogueActive;
    }
}
