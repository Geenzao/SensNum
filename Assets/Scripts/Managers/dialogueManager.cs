using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;


public class dialogueManager : Singleton<dialogueManager>
{
    public TextMeshProUGUI interactionKey;
    public TextMeshProUGUI dialogueTextUI; //texte qui serra modifi  avec les phrases des PNJ
    [SerializeField] private GameObject dialoguePanelUI; //Object UI du dialogue, ex : paneau gris ou appara t les phrase
    [SerializeField] private GameObject PanelUITextInteraction;

    private bool isDialogueActive = false;
    private Queue<string> qSentences;

    private dialoguePNJ dialoguePnjRef = null;

    private void Start()
    {
        PanelUITextInteraction.SetActive(false);
        dialoguePanelUI.SetActive(false);

        qSentences = new Queue<string>();

        interactionKey.text = LanguageManager.Instance.GetText("interaction_key");
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
        playerMovement.Instance.StopPlayerMouvement();
        dialoguePnjRef = diag;
        // On dit au Pnj de s'arrêter parce que le joueur lui parle
        if (dialoguePnjRef != null && dialoguePnjRef.gameObject.GetComponent<MouvementPNJ>())
            dialoguePnjRef.gameObject.GetComponent<MouvementPNJ>().PnjTalk();
        else
            Debug.LogWarning("Il y a un problème avec le scripte MouvementPNJ");

        isDialogueActive = true;
        qSentences.Clear();
        // On affiche l'UI du dialogue
        dialoguePanelUI.SetActive(true);
        int nbInteraction = diag.getInteractionCount();
        if (nbInteraction < diag.listDialogue.Count)
        {
            foreach (string sentence in diag.listDialogue[nbInteraction].sentences)
            {
                qSentences.Enqueue(sentence);
            }
        }
        else
        {
            // ICI ça veut dire qu'il y a une erreur dans le nombre de dialogue.
            // En cas d'erreur, on répète le dernier dialogue du PNJ en boucle
            nbInteraction = diag.listDialogue.Count - 1;
            foreach (string sentence in diag.listDialogue[nbInteraction].sentences)
            {
                qSentences.Enqueue(sentence);
            }
        }
        DisplayNextSentence();
    }

    public void ShowPanelInteraction()
    {
        PanelUITextInteraction.SetActive(true);
    }

    public void HidePanelInteraction()
    {
        PanelUITextInteraction.SetActive(false);
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
        dialoguePanelUI.SetActive(false);
        isDialogueActive = false;
        playerMovement.Instance.ActivePlayerMouvement();
        //On dit au Pnj de reprendre la marche
        if (dialoguePnjRef != null && dialoguePnjRef.gameObject.GetComponent<MouvementPNJ>())
            dialoguePnjRef.gameObject.GetComponent<MouvementPNJ>().PnjDontTalk();
        else
            Debug.LogWarning("Il y a un probl�me avec le scripte MouvementPNJ");

    }

    public bool fctisDialogueActive()
    {
        return isDialogueActive;
    }
}
