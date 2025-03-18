using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameManager;
using System;


public class DialogueManagerUI : MonoBehaviour
{

    public TextMeshProUGUI interactionKey;
    public TextMeshProUGUI dialogueTextUI; //texte qui serra modifi  avec les phrases des PNJ

    [SerializeField] private GameObject dialoguePanelUI; //Object UI du dialogue, ex : paneau gris ou appara t les phrase
    [SerializeField] private GameObject PanelUITextInteraction;

    //pour le bouton qui permet de passer les phrase
    public UnityEngine.UI.Button btnNextSentence;
    public GameObject btnNextSentenceVisuel;

    //pour le portable
    public UnityEngine.UI.Button btnInteraction; //pour le bouton d'interaction
    public GameObject textInteraction; //pour le texte E 
    public bool isMobilePlatform = false;


    private bool aPNJnormalaParler = false;
    private bool aPNJChefaParler = false;

    private dialoguePNJ dialoguePnjRef = null;
    private dialoguePNJChef dialoguePNJChefRef = null;

    private Queue<string> qSentences = new Queue<string>();

    private Coroutine currentCoroutine = null; // Référence à la coroutine actuelle


    private void Awake()
    {
        btnNextSentence.onClick.AddListener(() => dialogueManager.Instance.DisplayNextSentence());
        btnInteraction.onClick.AddListener(() => dialogueManager.Instance.BtnInteraction());

        LanguageManager.OnLanguageChanged += UpdateTexts;
        dialogueManager.Instance.OnDialogueStartChef += StartDialogueChef;
        dialogueManager.Instance.OnDialogueStartNormal += StartDialogue;
        dialogueManager.Instance.OnDialogueEndChef += EndDialogueChef;
        dialogueManager.Instance.OnDialogueEndNormal += EndDialogue;

        dialogueManager.Instance.OnInteractionPossible += ShowPanelInteraction;
        dialogueManager.Instance.OnInteractionImpossible += HidePanelInteraction;

        dialogueManager.Instance.OnNextSentence += DisplayNextSentence;

        dialogueManager.Instance.OnDialoguePNJnormalFinish += EndDialogue;
        dialogueManager.Instance.OnDialoguePNJChefFinish += EndDialogueChef;

        dialogueManager.Instance.OnHideDialoguePanel += EndDialogueChef;
    }

    void Start()
    {
        dialoguePanelUI.SetActive(false);
        PanelUITextInteraction.SetActive(false);
        isMobilePlatform = PlatformManager.Instance.fctisMobile();

        if (LanguageManager.Instance != null)
        {
            UpdateTexts();
        }
        else
        {
            Debug.LogError("LanguageManager instance is not initialized.");
        }
    }

    private void Update()
    {
        if(UIManager.CurrentMenuState != UIManager.MenuState.Dialogue)
        {
            dialoguePanelUI.SetActive(false);
        }
        if(UIManager.CurrentMenuState == UIManager.MenuState.Cinematic)
        {
            dialoguePanelUI.SetActive(false);
            PanelUITextInteraction.SetActive(false);
        }
    }

    public void UpdateTexts()
    {
        interactionKey.text = LanguageManager.Instance.GetText("interaction_key");
    }

    private void ShowPanelInteraction()
    {
        //pour le portable, on affiche l'interaction en fonction de portable ou non
        if (isMobilePlatform == true)
        {
            btnInteraction.gameObject.SetActive(true);
            textInteraction.SetActive(false);
        }
        else
        {
            btnInteraction.gameObject.SetActive(false);
            textInteraction.SetActive(true);
        }
        PanelUITextInteraction.SetActive(true);
    }

    public void StartDialogue()
    {
        //pour cacher le bouton ou le texte d'interaction
        HidePanelInteraction();


        qSentences.Clear();
        // On affiche l'UI du dialogue
        dialoguePanelUI.SetActive(true);

        //On récupère le dialogue auprès du dialogueManager
        dialoguePNJ diag = dialogueManager.Instance.GetDialoguePnjRef();

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

    public void StartDialogueChef()
    {
        HidePanelInteraction();

        qSentences.Clear();

        // On affiche l'UI du dialogue
        dialoguePanelUI.SetActive(true);

        //On récupère le dialogue auprès du dialogueManager
        dialoguePNJChef diag = dialogueManager.Instance.GetDialoguePNJChefRef();




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

    public void HidePanelInteraction()
    {
        //pour éviter une erreur
        if(PanelUITextInteraction != null)
            PanelUITextInteraction.SetActive(false);
    }

    public void ShowBtnNext()
    {
        if (btnNextSentenceVisuel != null)
            btnNextSentenceVisuel.SetActive(true);
        dialogueManager.Instance.SetIsAbleToNextSentence(true);
    }

    public void HideBtnNext()
    {
        btnNextSentenceVisuel.SetActive(false);
        dialogueManager.Instance.SetIsAbleToNextSentence(false);
    }

    public void DisplayNextSentence()
    {
        if (qSentences.Count == 0)
        {
            int a = dialogueManager.Instance.GetLastPNJType();
            if (a == 0)
                EndDialogue();
            else if (a == 1)
                EndDialogueChef();
            return;
        }
        HideBtnNext(); //on désaffiche le btn next sentence pour éviter à l'utilisateur de passer le diag sans lire

        string sentence = qSentences.Dequeue();
        //empecher affichage de plusieur phrase si appuy sur suivant avaant la fin de la premiere coroutine
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(LettreParLettre(sentence));
    }

    IEnumerator LettreParLettre(string sentence)
    {
        dialogueTextUI.text = "";
        foreach (char lettre in sentence.ToCharArray())
        {
            dialogueTextUI.text += lettre;
            yield return new WaitForSeconds(0.005f);
        }
        currentCoroutine = null; // La coroutine est terminée
        ShowBtnNext();//On affiche le btn à la fin
    }

    public void EndDialogue()
    {
        dialoguePanelUI.SetActive(false);
        dialogueManager.Instance.EndDialogue();
    }


    public void EndDialogueChef()
    {
        dialoguePanelUI.SetActive(false);
        dialogueManager.Instance.EndDialogueChef();
    }

}
