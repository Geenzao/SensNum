using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


/*
 * NOTE : Pour jouer un effet sonore, il suffit d�appeler la m�thode `PlaySoundEffect(int index)` 
 * d�finie dans la classe `AudioManager` ci-dessous.
 *
 * Utilisation :
 * - Ajoutez un son dans le tableau des effets sonores (inspecteur Unity).
 * - Utilisez l�indice correspondant pour jouer cet effet sonore.
 */

/**********************************************************
 * Note d�optimisation : Pour �viter des calculs de transition en temps r�el et am�liorer les performances, 
 * nous avons d�cid� de ne pas impl�menter de fondu automatique (fade) entre les musiques. 
 * � la place, nous utilisons des fichiers .wav modifi�s pour int�grer des crescendos et decrescendos au d�but et � la fin de chaque morceau.
 */


public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Clips Music")]
    //Chaque Tuple contient 2 AudioClips, le premier est la musique de d�but, le deuxi�me est la musique de boucle
    public List<CTuple<AudioClip,AudioClip>> tabMusic;


    [Header("Audio Clips Sound Effect")]
    public AudioClip[] tabSoundEffect;

    [Header("Audio Sources")]
    public AudioSource audioSource; //responsable de jouer les musiques de fond
    public AudioMixerGroup soundEffectMixer;//Mixer pour les effets sonores

    //pour suivre la musique qui est jouer
    private int musicStateIndex = -1;
    private int musicToPlay = 0;


    void Start()
    {
        GameProgressManager.Instance.OnGameProgressStateChange.AddListener(HandleGameProgressStateChanged);
    }

    //Il faudrait qu'en fonction du GameProgressState, on joue une musique diff�rente, en premier la musique de d�but, puis la musique de boucle en boucle
    private void Update()
    {
        if (audioSource.isPlaying == false)
        {
            if (musicStateIndex >= 0 && musicStateIndex < tabMusic.Count)
            {
                if (musicToPlay == 0)
                {
                    audioSource.clip = tabMusic[musicStateIndex].Item1;
                    audioSource.Play();
                    musicToPlay = 1;
                } else {
                    audioSource.clip = tabMusic[musicStateIndex].Item2;
                    audioSource.Play();
                }
            }
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    //Cette fonction permet de jouer un effet sonnor
    //Les autre classe on juste � l'appeler et � renseigner en param l'indice de l'effet sonnor plac� dans l'AudioManager
    public AudioSource PlaySoundEffet(int index)
    {
        if (index < 0 || index > tabSoundEffect.Length)
        {
            Debug.LogWarning("Index d'effet sonore invalide !");
            return null;
        }

        GameObject objectTempo = new GameObject("TempAudio");
        AudioSource audioSource = objectTempo.AddComponent<AudioSource>();//on ajoute un audio source au game object
        audioSource.clip = tabSoundEffect[index];
        audioSource.outputAudioMixerGroup = soundEffectMixer; // on ajoute le mixer de l'audio source
        audioSource.Play();
        Destroy(objectTempo, tabSoundEffect[index].length); // on d�truit l'objet une fois qu'on arrive a la fin du clip audio de l'objet
        return audioSource;
    }

    private void HandleGameProgressStateChanged(GameProgressManager.GameProgressState newGameProgressState, GameProgressManager.GameProgressState oldGameProgressState)
    {
        switch (newGameProgressState)
        {
            case GameProgressManager.GameProgressState.Start:
                musicStateIndex = 0;
                musicToPlay = 0;
                StopMusic();
                break;
            //case GameProgressManager.GameProgressState.Menu:
            //    musicStateIndex = 1;
            //    musicToPlay = 0;
            //    StopMusic();
            //    break;
            //case GameProgressManager.GameProgressState.Mine:
            //    musicStateIndex = 2;
            //    break;
            //case GameProgressManager.GameProgressState.Factory:
            //    musicStateIndex = 3;
            //    break;
            //case GameProgressManager.GameProgressState.Residence:
            //    musicStateIndex = 4;
            //    break;
            //case GameProgressManager.GameProgressState.End:
            //    musicStateIndex = 5;
            //    break;
            default:
                break;
        }
    }
}
