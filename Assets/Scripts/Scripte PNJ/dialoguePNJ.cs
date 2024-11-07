

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    [TextArea(3, 10)]
    public string[] sentences;  // tableau de phrases pour un ensemble de dialogue
}

public class dialoguePNJ : MonoBehaviour
{
    public Dialogue[] tabDialogue;

    // Compteur pour suivre le nombre d'interactions avec le PNJ
    private int interactionCount = 0;
    private bool range = false; //pour savoir si le joueur est assez proche

    void Update()
    {
        if (dialogueManager.Instance.fctisDialogueActive() == false && range == true && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.Instance.StartDialogue(this);
            incrementeInteractionCount(); // Incr mente le compteur d'interactions pour signifier qu'on a lanc  le premier dialogue
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            range = true;
            dialogueManager.Instance.ShowPanelInteraction();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            range = false;
            dialogueManager.Instance.HidePanelInteraction();
            dialogueManager.Instance.EndDialogue();
        }
    }

    public int getInteractionCount()
    {
        return interactionCount;
    }

    public void incrementeInteractionCount()
    {
        // a ne sert   rien d'incr menter   l'infini
        if (interactionCount + 1 < tabDialogue.Length + 1)
            interactionCount++;
    }

}




