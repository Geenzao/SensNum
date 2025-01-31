

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
    public List<Dialogue> listDialogue = new List<Dialogue>();

    //Récupère le tag du PNJ
    [SerializeField] private new string name;
    // Compteur pour suivre le nombre d'interactions avec le PNJ
    private int interactionCount = 0;
    private bool range = false; //pour savoir si le joueur est assez proche

    private bool finishAlreadyReached = false;
    [SerializeField] private PathManager.PathState path;
    [SerializeField] private bool changePathState;

    void Awake()
    {
        LanguageManager.OnLanguageChanged += InitializeDialogue;
        InputManager.Instance.OnUserActionDialogue += LancerDialogue;
    }

    void Start()
    {
        InitializeDialogue();
    }

    void InitializeDialogue()
    {
        listDialogue.Clear();
        int i = 0;
        while (true)
        {
            List<string> sentences = new List<string>();
            int j = 0;
            while (true)
            {
                string key = $"pnj_{name}_{i}_{j}";
                string text = LanguageManager.Instance.GetText(key);
                if (text == key) // Si le texte retourné est la clé, cela signifie qu'il n'y a plus de texte pour cette catégorie
                {
                    break;
                }
                sentences.Add(text);
                j++;
            }
            if (sentences.Count == 0) // Si aucune phrase n'a été ajoutée, on arrête la boucle
            {
                break;
            }
            Dialogue dialogue = new Dialogue { sentences = sentences.ToArray() };
            listDialogue.Add(dialogue);
            i++;
        }
    }

    //void Update()
    //{
    //    if (dialogueManager.Instance.fctisDialogueActive() == false && range == true && Input.GetKeyDown(KeyCode.E))
    //    {
    //        dialogueManager.Instance.StartDialogue(this);
    //        incrementeInteractionCount(); // Incrémente le compteur d'interactions pour signifier qu'on a lancé le premier dialogue

    //        if (IsLastDialogue())
    //        {
    //            Finish();
    //        }
    //    }
    //}

    private void LancerDialogue()
    {
        if(dialogueManager.Instance.fctisDialogueActive() == false && range == true)
        {
            dialogueManager.Instance.StartDialogue(this);
            incrementeInteractionCount(); // Incrémente le compteur d'interactions pour signifier qu'on a lancé le premier dialogue

            if (IsLastDialogue())
            {
                Finish();
            }
        }
    }

    private bool IsLastDialogue()
    {
        return interactionCount >= listDialogue.Count;
    }

    private void Finish()
    {
        if (!finishAlreadyReached)
        {
            if (gameObject.GetComponent<warningPNJ>())
                gameObject.GetComponent<warningPNJ>().hideWarning();

            finishAlreadyReached = true;
            if (changePathState)
            {
                PathManager.Instance.UpdatePathState(path);
                InputManager.invokeOnPathStateChanged();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            range = true;
            dialogueManager.Instance.ShowPanelInteractionPNJnormal(this);
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
        if (interactionCount + 1 < listDialogue.Count + 1)
            interactionCount++;
    }

    public void CheckifEnd()
    {
        if (IsLastDialogue())
        {
            Finish();
        }
    }
}




