using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;


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
    }

    [Header("UI")]
    public GameObject PanelDialogueCinematicUI;
    public TextMeshProUGUI txtDialogueCinematic;

    private int currentCinematicClipIndex = 0; // Index du clip actuel
    private int currentSentenceIndex = 0; // Index de la phrase actuelle

    private Coroutine currentCoroutine = null; // Référence à la coroutine actuelle


    public CinematicBloc[] tabCinematicBloc;

    private void Start()
    {
        // Initialisation des clips et calcul des durées des dialogues
        foreach (var bloc in tabCinematicBloc)
        {
            bloc.clips.isRening = false;
            bloc.clips.gameObject.SetActive(false);
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
            //Debug.Log("Toutes les cinématiques sont terminées !");
            return;
        }


        // Si clic de souris, gérer le dialogue ou changer de cinématique
        if (Input.GetMouseButtonDown(0)) // Clic gauche de la souris
        {
            HandleDialogueOrClipTransition();
        }
    }

    private void HandleDialogueOrClipTransition()
    {
        CinematicBloc currentBloc = tabCinematicBloc[currentCinematicClipIndex];

        // Si des phrases restent à afficher
        if (currentSentenceIndex < currentBloc.dialogueCinematic.sentences.Length)
        {
            DisplaySentence(currentSentenceIndex);
            currentSentenceIndex++;
        }
        else
        {
            // Si toutes les phrases ont été affichées, passer au clip suivant
            EndCinematic(currentCinematicClipIndex);
            currentCinematicClipIndex++;

            if (currentCinematicClipIndex < tabCinematicBloc.Length)
            {
                StartCinematic(currentCinematicClipIndex);
            }
            else
            {
                //Debug.Log("Toutes les cinématiques ont été jouées !");
            }
        }
    }

    private void StartCinematic(int index)
    {
        //Debug.Log($"Démarrage du clip : {tabCinematicBloc[index].clips.gameObject.name}");

        var currentBloc = tabCinematicBloc[index];
        currentSentenceIndex = 0; // Réinitialiser l'indice des phrases

        // Activer le clip et le panneau de dialogue
        currentBloc.clips.gameObject.SetActive(true);
        currentBloc.clips.isRening = true;
        PanelDialogueCinematicUI.SetActive(true);

        // Afficher la première phrase
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

        // Désactiver le panneau de dialogue
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

        currentCoroutine = null; // La coroutine est terminée
    }
}


