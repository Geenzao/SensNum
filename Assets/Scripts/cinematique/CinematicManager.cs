using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;


/*
 * Avant chaque mini jeux, il y a une cinématique d'explication. 
 * Pour chaque cinématique, il y a un ensemble de CinematicClip qui sont géré par le CinematicManager
 * 
 * Un CinematicBloc permet de lié un clip et un dialogue pour difusé des phrases pendant le clip
 * 
 * Cette classe CinematicManager est responssable de jouer les cinematique 
 * et de coordoné les dialogue dans le future qui seront afficher dessus
 *
 */

public class CinematicManager : MonoBehaviour
{
    //Le bloc permet de lié un clip à un dialogue
    [System.Serializable]
    public class CinematicBloc
    {
        [Header("Tableau des Clips")]
        public CinematicClip clips; // Un clip 

        [Header("Dialogue Cinematic")]
        public Dialogue dialogueCinematic; // Sera diffusé pendant le clip en parallèle

        public float durationClip = 1.0f; // temps du clip à l'écran

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
    private float timeElapsedClip = 0f; // Temps écoulé pour le clip actuel
    private float timeElapsedDialogue = 0f; // Temps écoulé pour le dialogue

    public CinematicBloc[] tabCinematicBloc;

    private void Start()
    {
        // Initialisation des clips et calcul des durées des dialogues
        foreach (var bloc in tabCinematicBloc)
        {
            bloc.clips.isRening = false;
            bloc.clips.gameObject.SetActive(false);
            bloc.setTimeDialogue(bloc.durationClip / bloc.dialogueCinematic.sentences.Length);
        }

        PanelDialogueCinematicUI.SetActive(false);

        // Démarrage de la première cinématique
        if (tabCinematicBloc.Length > 0)
        {
            StartCinematic(0);
        }
    }

    private void Update()
    {
        // Si toutes les cinématiques sont jouées, on arrête
        if (currentCinematicClipIndex >= tabCinematicBloc.Length)
        {
            Time.timeScale = 0;
            Debug.Log("Toutes les cinématiques sont terminées !");
            return;
        }

        // Mise à jour du temps écoulé
        timeElapsedClip += Time.deltaTime;
        timeElapsedDialogue += Time.deltaTime;

        // Gestion des dialogues
        var currentBloc = tabCinematicBloc[currentCinematicClipIndex];
        if (currentSentenceIndex < currentBloc.dialogueCinematic.sentences.Length &&
            timeElapsedDialogue >= currentBloc.getTimeDialogue())
        {
            DisplaySentence(currentSentenceIndex);
            currentSentenceIndex++;
            timeElapsedDialogue = 0f; // Réinitialisation du temps pour la prochaine phrase
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
        Debug.Log($"Démarrage du clip : {tabCinematicBloc[index].clips.gameObject.name}");

        var currentBloc = tabCinematicBloc[index];
        timeElapsedClip = 0f;
        timeElapsedDialogue = 0f;
        currentSentenceIndex = 0;

        // Activation du clip et du panneau de dialogue
        currentBloc.clips.gameObject.SetActive(true);
        currentBloc.clips.isRening = true;
        PanelDialogueCinematicUI.SetActive(true);

        // Affichage de la première phrase
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

        // Désactivation du panneau de dialogue
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


