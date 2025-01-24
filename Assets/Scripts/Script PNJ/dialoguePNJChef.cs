using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class dialoguePNJChef : MonoBehaviour
{
    public List<Dialogue> listDialogue = new List<Dialogue>();

    //Récupère le tag du PNJ
    [SerializeField] private new string name;
    // Compteur pour suivre le nombre d'interactions avec le PNJ
    private int interactionCount = 0;
    private bool range = false; //pour savoir si le joueur est assez proche

    private bool finishAlreadyReached = false;
    [SerializeField] private PathManager.PathState path;

    [Header("Story advancement")]
    [SerializeField] private string actualScene;
    [SerializeField] private string sceneCinematiqueToLoad;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private GameProgressManager.GameProgressState beginingGameProgressState;
    [SerializeField] private GameProgressManager.GameProgressState endGameProgressState;


    void Awake()
    {
        LanguageManager.Instance.OnLanguageChanged += InitializeDialogue;
    }

    void Start()
    {
        StartCoroutine(InitializeDialogueCoroutine());
        InitializeDialogue();
    }

    IEnumerator InitializeDialogueCoroutine()
    {
        yield return new WaitForSeconds(7f);
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
                string key;
                //Debug.Log(GameProgressManager.CurrentGameProgressState);
                //Debug.Log(beginingGameProgressState);
                if (GameProgressManager.CurrentGameProgressState == beginingGameProgressState)
                     key = $"pnj_{name}_{i}_{j}";
                else
                {
                    if (GameProgressManager.CurrentGameProgressState == endGameProgressState)
                        key = $"pnj_{name}_end_{i}_{j}";
                    else
                        key = $"pnj_{name}_boucle_{i}_{j}";
                }
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

    void Update()
    {
        if (dialogueManager.Instance.fctisDialogueActive() == false && range == true && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.Instance.StartDialogueChef(this);
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
            finishAlreadyReached = true;
            if (GameProgressManager.CurrentGameProgressState == beginingGameProgressState)
            {
                GameManager.Instance.UnloadAndSavePosition(actualScene, x, y);
                GameManager.Instance.LoadLevel(sceneCinematiqueToLoad);
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.None);
            }
            else
                if (GameProgressManager.CurrentGameProgressState == endGameProgressState)
                    PathManager.Instance.UpdatePathState(path);
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
            dialogueManager.Instance.EndDialogueChef();
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

}




