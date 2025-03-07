using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using System;



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
        {
            pnj.incrementeInteractionCount();
            pnj.CheckifEnd();

        }
        else if (pnjChef != null)
        {
            pnjChef.incrementeInteractionCount();
            pnjChef.CheckifEnd();
        }
    }
}


public class dialogueManager : Singleton<dialogueManager>
{
    //Event pour dire au dialogueManagerUI que le dialogue commence
    public event Action OnDialogueStartChef;
    public event Action OnDialogueStartNormal;
    public event Action OnDialogueEndChef;
    public event Action OnDialogueEndNormal;

    //pour afficher l'interaction possible
    public event Action OnInteractionPossible;
    public event Action OnInteractionImpossible;

    //pour passer a la phrase suivante
    public event Action OnNextSentence;

    //pour la fin des dialogue
    public event Action OnDialoguePNJnormalFinish;
    public event Action OnDialoguePNJChefFinish;

    public event Action OnHideDialoguePanel;



    public bool isAbleToNextSentence = false;

    //pour le portable
    public bool isMobilePlatform = false;
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
        qSentences = new Queue<string>();
        isMobilePlatform = PlatformManager.Instance.fctisMobile();
    }

    public void BtnInteraction()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
        //print("On a lické sur le btn");
        //print(lastPNJ.getTypeDialogue());
        //pour réglé le bug où on clic sur le bouton et que le menu paramètre est afficher
        if (GameManager.CurrentGameState == GameState.PAUSED)
            return;

        

        if (lastPNJ.getTypeDialogue() == 0)
            StartDialogue(lastPNJ.getLastPNJnormal());
        else if (lastPNJ.getTypeDialogue() == 1)
            StartDialogueChef(lastPNJ.getLastPNJchef());
        else
            Debug.LogWarning("Il n'y a pas de dialogue à afficher");
    }

    public void StartDialogue(dialoguePNJ diag)
    {
        UIManager.Instance.UpdateMenuState(UIManager.MenuState.Dialogue);
        aPNJnormalaParler = true;

        playerMovement.Instance.StopPlayerMouvement();
        dialoguePnjRef = diag;

        // On dit au Pnj de s'arrêter parce que le joueur lui parle
        if (dialoguePnjRef != null && dialoguePnjRef.gameObject.GetComponent<MouvementPNJ>())
            dialoguePnjRef.gameObject.GetComponent<MouvementPNJ>().PnjTalk();
        else
            Debug.LogWarning("Il y a un problème avec le scripte MouvementPNJ");

        isDialogueActive = true;

        OnDialogueStartNormal?.Invoke();
    }

    public dialoguePNJ GetDialoguePnjRef()
    {
        return dialoguePnjRef;
    }

    public dialoguePNJChef GetDialoguePNJChefRef()
    {
        return dialoguePNJChefRef;
    }

    public void StartDialogueChef(dialoguePNJChef diag)
    {
        UIManager.Instance.UpdateMenuState(UIManager.MenuState.Dialogue);
        aPNJChefaParler = true;

        playerMovement.Instance.StopPlayerMouvement();
        dialoguePNJChefRef = diag;
        // On dit au Pnj de s'arrêter parce que le joueur lui parle
        if (dialoguePNJChefRef != null && dialoguePNJChefRef.gameObject.GetComponent<MouvementPNJ>())
            dialoguePNJChefRef.gameObject.GetComponent<MouvementPNJ>().PnjTalk();
        else
            Debug.LogWarning("Il y a un problème avec le scripte MouvementPNJ");

        isDialogueActive = true;

        OnDialogueStartChef?.Invoke();
    }

    public void ShowPanelInteractionPNJnormal(dialoguePNJ pnj = null)
    {
        if(pnj != null)
        lastPNJ.setLastPNJnormal(pnj);

        if (pnj == null)
            dialoguePnjRef = pnj;

        OnInteractionPossible?.Invoke();
    }

    public void HidePanelInteraction()
    {
        OnInteractionImpossible?.Invoke();
    }

    public void ShowPanelInteractionPNJchef(dialoguePNJChef pnjChef = null )
    {
        if (pnjChef != null)
        lastPNJ.setLastPNJchef(pnjChef);

        if(pnjChef != null)
        dialoguePNJChefRef = pnjChef;

        OnInteractionPossible?.Invoke();
    }

    public int GetLastPNJType()
    {
        return lastPNJ.getTypeDialogue();
    }
    public void SetIsAbleToNextSentence(bool a)
    {
        isAbleToNextSentence = a;
    }

    public void DisplayNextSentence()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
        OnNextSentence?.Invoke();
    }


    public void EndDialogue()
    {
        print("EndDialogue");
        UIManager.Instance.UpdateMenuState(UIManager.MenuState.None);
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
            OnInteractionPossible?.Invoke();
            aPNJnormalaParler = false;
            //on increment l'index du pnj
            if(isMobilePlatform)
            {
                lastPNJ.incrementInteractionCount();
            }
        }
    }


    public void EndDialogueChef()
    {
        UIManager.Instance.UpdateMenuState(UIManager.MenuState.None);
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
            OnInteractionPossible?.Invoke();
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

    public void hideDialogPanel()
    {
        OnHideDialoguePanel?.Invoke();
    }

}












