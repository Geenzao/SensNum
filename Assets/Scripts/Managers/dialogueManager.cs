using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;


//Cette classe sert à garder en mémoire le dialogue du dernière interaction avec un PNJ
public class lastPNJDialogueContener
{
    public dialoguePNJ pnj = null;
    public dialoguePNJChef pnjChef = null;

    public void setLastPNJnormal(dialoguePNJ pnj)
    {
        this.pnj = pnj; //on set le nouveeau dialogue
        this.pnjChef = null; //on set le dialogue chef à null pour dire que ce n'est plus le dernier
    }

    public void setLastPNJchef(dialoguePNJChef pnjChef)
    {
        this.pnjChef = pnjChef;
        this.pnj = null;
    }

    public dialoguePNJ getLastPNJnormal()
    {
        return pnj;
    }

    public dialoguePNJChef getLastPNJchef()
    {
        return pnjChef;
    }

    //pour savoir c'est quel type de dialogue qui est le dernier
    // 0 = dialogue normal
    // 1 = dialogue chef
    public int getTypeDialogue()
    {
        if (pnj != null)
            return 0;
        else if (pnjChef != null)
            return 1;
        else
            return -1;
    }

    public void incrementInteractionCount()
    {
        if (pnj != null)
            pnj.incrementeInteractionCount();
        else if (pnjChef != null)
            pnjChef.incrementeInteractionCount();
    }
}


public class dialogueManager : Singleton<dialogueManager>
{
    public TextMeshProUGUI interactionKey;
    public TextMeshProUGUI dialogueTextUI; //texte qui serra modifi  avec les phrases des PNJ
    [SerializeField] private GameObject dialoguePanelUI; //Object UI du dialogue, ex : paneau gris ou appara t les phrase
    [SerializeField] private GameObject PanelUITextInteraction;

    //pour le portable
    public bool isMobilePlatform = false;
    public Button btnInteraction; //pour le bouton d'interaction
    public GameObject textInteraction; //pour le texte E 
    private lastPNJDialogueContener lastPNJ = new lastPNJDialogueContener();

    private bool aPNJnormalaParler = false;
    private bool aPNJChefaParler = false;

    private bool isDialogueActive = false;
    private Queue<string> qSentences;

    private dialoguePNJ dialoguePnjRef = null;
    private dialoguePNJChef dialoguePNJChefRef = null;

    private Coroutine currentCoroutine = null; // Référence à la coroutine actuelle

    private void Start()
    {
        LanguageManager.Instance.OnLanguageChanged += UpdateTexts;

        UpdateTexts();

        PanelUITextInteraction.SetActive(false);
        dialoguePanelUI.SetActive(false);

        qSentences = new Queue<string>();
        isMobilePlatform = PlatformManager.Instance.fctisMobile();

        btnInteraction.onClick.AddListener(() =>
        {
            print("On a lické sur le btn");
            if (lastPNJ.getTypeDialogue() == 0)
                StartDialogue(lastPNJ.getLastPNJnormal());
            else if (lastPNJ.getTypeDialogue() == 1)
                StartDialogueChef(lastPNJ.getLastPNJchef());
            else
                Debug.LogWarning("Il n'y a pas de dialogue à afficher");
        });
    }

    //private void Update()
    //{
    //    if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        DisplayNextSentence();
    //    }
    //}

    public void StartDialogue(dialoguePNJ diag)
    {
        aPNJnormalaParler = true;
        //pour cacher le bouton ou le texte d'interaction
        HidePanelInteraction();

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

    public void StartDialogueChef(dialoguePNJChef diag)
    {
        aPNJChefaParler = true;
        print("StartDialogueChef");
        //pour cacher le bouton ou le texte d'interaction
        HidePanelInteraction();

        playerMovement.Instance.StopPlayerMouvement();
        dialoguePNJChefRef = diag;
        // On dit au Pnj de s'arrêter parce que le joueur lui parle
        if (dialoguePNJChefRef != null && dialoguePNJChefRef.gameObject.GetComponent<MouvementPNJ>())
            dialoguePNJChefRef.gameObject.GetComponent<MouvementPNJ>().PnjTalk();
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

    public void ShowPanelInteractionPNJnormal(dialoguePNJ pnj = null)
    {
        if(pnj != null)
        lastPNJ.setLastPNJnormal(pnj);

        if (pnj == null)
            dialoguePnjRef = pnj;

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

    public void ShowPanelInteractionPNJchef(dialoguePNJChef pnjChef = null )
    {
        if (pnjChef == null)
        lastPNJ.setLastPNJchef(pnjChef);

        if(pnjChef != null)
        dialoguePNJChefRef = pnjChef;

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

    public void HidePanelInteraction()
    {

        PanelUITextInteraction.SetActive(false);
    }

    public void DisplayNextSentence()
    {
        if (qSentences.Count == 0)
        {
            if(dialoguePnjRef != null)
                EndDialogue();
            else if (dialoguePNJChefRef != null)
                EndDialogueChef();
            return;
        }

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
            yield return new WaitForSeconds(0.01f);
        }
        currentCoroutine = null; // La coroutine est terminée
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
            Debug.LogWarning("Il y a un problème avec le script MouvementPNJ");

        //On remet le bouton ou le texte d'interaction*
        if(aPNJnormalaParler == true)
        {
            ShowPanelInteraction();
            aPNJnormalaParler = false;
            //on increment l'index du pnj
            if(isMobilePlatform)
            {
                lastPNJ.incrementInteractionCount();
            }
        }

        print("ShowPanelInteraction + EndDialogue");
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

    public void EndDialogueChef()
    {
        dialoguePanelUI.SetActive(false);
        isDialogueActive = false;
        playerMovement.Instance.ActivePlayerMouvement();
        //On dit au Pnj de reprendre la marche
        if (dialoguePNJChefRef != null && dialoguePNJChefRef.gameObject.GetComponent<MouvementPNJ>())
            dialoguePNJChefRef.gameObject.GetComponent<MouvementPNJ>().PnjDontTalk();
        else
            Debug.LogWarning("Il y a un problème avec le script MouvementPNJ");

        //On remet le bouton ou le texte d'interaction*
        if(aPNJChefaParler == true)
        {
            ShowPanelInteraction();
            aPNJChefaParler = false;
            if (isMobilePlatform)
            {
                lastPNJ.incrementInteractionCount();
            }
        }
    }

    public bool fctisDialogueActive()
    {
        return isDialogueActive;
    }

    public void UpdateTexts()
    {
        interactionKey.text = LanguageManager.Instance.GetText("interaction_key");
    }
}












