using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;


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

        public float durationClip = 1.0f; // temps du clip � l'�cran

        // pour le temps que seront afficher les phrases
        private float timeDialogue;
        public float getTimeDialogue() { return timeDialogue; }
        public void setTimeDialogue(float t) { timeDialogue = t; }
    }

    [Header("UI")]
    public GameObject PanelDialogueCinematicUI;
    public TextMeshProUGUI txtDialogueCinematic;

    private int currentCinematicClipIndex = 0; // Index du clip actuel
    private int currentSentenceIndex = 0; // Index de la phrase actuelle
    private float timeElapsedClip = 0f; // Temps �coul� pour le clip actuel
    private float timeElapsedDialogue = 0f; // Temps �coul� pour le dialogue

    public CinematicBloc[] tabCinematicBloc;

    private void Start()
    {
        // Initialisation des clips et calcul des dur�es des dialogues
        foreach (var bloc in tabCinematicBloc)
        {
            bloc.clips.isRening = false;
            bloc.clips.gameObject.SetActive(false);
            bloc.setTimeDialogue(bloc.durationClip / bloc.dialogueCinematic.sentences.Length);
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
            Debug.Log("Toutes les cin�matiques sont termin�es !");
            return;
        }

        // Mise � jour du temps �coul�
        timeElapsedClip += Time.deltaTime;
        timeElapsedDialogue += Time.deltaTime;

        // Gestion des dialogues
        var currentBloc = tabCinematicBloc[currentCinematicClipIndex];
        if (currentSentenceIndex < currentBloc.dialogueCinematic.sentences.Length &&
            timeElapsedDialogue >= currentBloc.getTimeDialogue())
        {
            DisplaySentence(currentSentenceIndex);
            currentSentenceIndex++;
            timeElapsedDialogue = 0f; // R�initialisation du temps pour la prochaine phrase
        }

        // Gestion de la fin du clip
        if (timeElapsedClip >= currentBloc.durationClip)
        {
            EndCinematic(currentCinematicClipIndex);
            currentCinematicClipIndex++;

            if (currentCinematicClipIndex < tabCinematicBloc.Length)
            {
                StartCinematic(currentCinematicClipIndex);
            }
        }
    }

    private void StartCinematic(int index)
    {
        Debug.Log($"D�marrage du clip : {tabCinematicBloc[index].clips.gameObject.name}");

        var currentBloc = tabCinematicBloc[index];
        timeElapsedClip = 0f;
        timeElapsedDialogue = 0f;
        currentSentenceIndex = 0;

        // Activation du clip et du panneau de dialogue
        currentBloc.clips.gameObject.SetActive(true);
        currentBloc.clips.isRening = true;
        PanelDialogueCinematicUI.SetActive(true);

        // Affichage de la premi�re phrase
        if (currentBloc.dialogueCinematic.sentences.Length > 0)
        {
            DisplaySentence(0);
        }
    }

    private void EndCinematic(int index)
    {
        Debug.Log($"Fin du clip : {tabCinematicBloc[index].clips.gameObject.name}");

        var currentBloc = tabCinematicBloc[index];
        currentBloc.clips.gameObject.SetActive(false);
        currentBloc.clips.isRening = false;

        // D�sactivation du panneau de dialogue
        PanelDialogueCinematicUI.SetActive(false);
    }

    private void DisplaySentence(int sentenceIndex)
    {
        var dialogue = tabCinematicBloc[currentCinematicClipIndex].dialogueCinematic;

        if (sentenceIndex < dialogue.sentences.Length)
        {
            string txt = dialogue.sentences[sentenceIndex];
            txtDialogueCinematic.text = txt;
            Debug.Log($"Affichage de la phrase : {dialogue.sentences[sentenceIndex]}");
        }
    }
}


