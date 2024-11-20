using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;


/*
 * Avant chaque mini jeux, il y a une cin�matique d'explication. 
 * Pour chaque cin�matique, il y a un ensemble de CinematicClip qui sont g�r� par le CinematicManager
 * 
 * Un CinematicBloc permet de li� un clip et un dialogue pour difus� des phrases pendant le clip
 * 
 * Cette classe CinematicManager est responssable de jouer les cinematique 
 * et de coordon� les dialogue dans le future qui seront afficher dessus
 *
 */

public class CinematicManager : MonoBehaviour
{
    //Le bloc permet de li� un clip � un dialogue
    [System.Serializable]
    public class CinematicBloc
    {
        [Header("Tableau des Clips")]
        public CinematicClip clips; // Un clip 

        [Header("Dialogue Cinematic")]
        public Dialogue dialogueCinematic; // Sera diffus� pendant le clip en parall�le
    }

    [Header("UI")]
    public GameObject PanelDialogueCinematicUI;
    public TextMeshProUGUI txtDialogueCinematic;

    private int currentCinematicClipIndex = 0; // Index du clip actuel
    private int currentSentenceIndex = 0; // Index de la phrase actuelle

    private Coroutine currentCoroutine = null; // R�f�rence � la coroutine actuelle


    public CinematicBloc[] tabCinematicBloc;

    private void Start()
    {
        // Initialisation des clips et calcul des dur�es des dialogues
        foreach (var bloc in tabCinematicBloc)
        {
            bloc.clips.isRening = false;
            bloc.clips.gameObject.SetActive(false);
        }

        PanelDialogueCinematicUI.SetActive(false);

        // D�marrage de la premi�re cin�matique
        if (tabCinematicBloc.Length > 0)
        {
            StartCinematic(0);
        }
    }

    private void Update()
    {
        // Si toutes les cin�matiques sont jou�es, on arr�te
        if (currentCinematicClipIndex >= tabCinematicBloc.Length)
        {
            Time.timeScale = 0;
            //Debug.Log("Toutes les cin�matiques sont termin�es !");
            return;
        }


        // Si clic de souris, g�rer le dialogue ou changer de cin�matique
        if (Input.GetMouseButtonDown(0)) // Clic gauche de la souris
        {
            HandleDialogueOrClipTransition();
        }
    }

    private void HandleDialogueOrClipTransition()
    {
        CinematicBloc currentBloc = tabCinematicBloc[currentCinematicClipIndex];

        // Si des phrases restent � afficher
        if (currentSentenceIndex < currentBloc.dialogueCinematic.sentences.Length)
        {
            DisplaySentence(currentSentenceIndex);
            currentSentenceIndex++;
        }
        else
        {
            // Si toutes les phrases ont �t� affich�es, passer au clip suivant
            EndCinematic(currentCinematicClipIndex);
            currentCinematicClipIndex++;

            if (currentCinematicClipIndex < tabCinematicBloc.Length)
            {
                StartCinematic(currentCinematicClipIndex);
            }
            else
            {
                //Debug.Log("Toutes les cin�matiques ont �t� jou�es !");
            }
        }
    }

    private void StartCinematic(int index)
    {
        //Debug.Log($"D�marrage du clip : {tabCinematicBloc[index].clips.gameObject.name}");

        var currentBloc = tabCinematicBloc[index];
        currentSentenceIndex = 0; // R�initialiser l'indice des phrases

        // Activer le clip et le panneau de dialogue
        currentBloc.clips.gameObject.SetActive(true);
        currentBloc.clips.isRening = true;
        PanelDialogueCinematicUI.SetActive(true);

        // Afficher la premi�re phrase
        if (currentBloc.dialogueCinematic.sentences.Length > 0)
        {
            DisplaySentence(0);
            currentSentenceIndex++;
        }
    }

    private void EndCinematic(int index)
    {
        //Debug.Log($"Fin du clip : {tabCinematicBloc[index].clips.gameObject.name}");
        var currentBloc = tabCinematicBloc[index];
        currentBloc.clips.gameObject.SetActive(false);
        currentBloc.clips.isRening = false;

        // D�sactiver le panneau de dialogue
        PanelDialogueCinematicUI.SetActive(false);
    }

    private void DisplaySentence(int sentenceIndex)
    {
        Dialogue dialogue = tabCinematicBloc[currentCinematicClipIndex].dialogueCinematic;

        if (sentenceIndex < dialogue.sentences.Length)
        {
            //empecher affichage de plusieur phrase si appuy sur suivant avaant la fin de la premiere coroutine
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(LettreParLettre(dialogue.sentences[sentenceIndex]));
            //Debug.Log($"Affichage de la phrase : {dialogue.sentences[sentenceIndex]}");
        }
    }

    IEnumerator LettreParLettre(string sentence)
    {
        txtDialogueCinematic.text = "";
        foreach (char lettre in sentence.ToCharArray())
        {
            txtDialogueCinematic.text += lettre;
            yield return new WaitForSeconds(0.005f);
        }

        currentCoroutine = null; // La coroutine est termin�e
    }
}


