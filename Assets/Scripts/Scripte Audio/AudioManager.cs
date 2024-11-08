using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


/*
 * NOTE : Pour jouer un effet sonore, il suffit d’appeler la méthode `PlaySoundEffect(int index)` 
 * définie dans la classe `AudioManager` ci-dessous.
 *
 * Utilisation :
 * - Ajoutez un son dans le tableau des effets sonores (inspecteur Unity).
 * - Utilisez l’indice correspondant pour jouer cet effet sonore.
 */

/**********************************************************
 * Note d’optimisation : Pour éviter des calculs de transition en temps réel et améliorer les performances, 
 * nous avons décidé de ne pas implémenter de fondu automatique (fade) entre les musiques. 
 * À la place, nous utilisons des fichiers .wav modifiés pour intégrer des crescendos et decrescendos au début et à la fin de chaque morceau.
 */


public class AudioManager : MonoBehaviour
{
    //Pour stocker les musiques qui seront jouer en boucle en fond
    public AudioClip[] tabMusics;
    //Pour stocker les effets sonnors
    public AudioClip[] tabSoundEffect;

    public AudioSource audioSource; //responsable de jouer les musiques de fond
    public AudioMixerGroup soundEffectMixer;//Mixer pour les effets sonores

    //pour suivre la musique qui est jouer
    private int musicIndex;
    // Sauvegarde de la position de la musique
    private float lastPosition = 0f;        


    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        audioSource.clip = tabMusics[0];
        audioSource.Play();
    }



    void Update()
    {
        if (!audioSource.isPlaying)
            PlayNextSong();
    }

    void PlayNextSong()
    {
        //On incrémente l'index de la musique en cour
        //Si musicIndex est supérieur à la taille du tableaa, l'index revien à 0
        musicIndex = (musicIndex + 1) % tabMusics.Length;
        audioSource.clip = tabMusics[musicIndex];
        audioSource.Play();
    }


    //Si on veut soudainement jouer une autre musique spécial
    public void PlayMusicByIndex(int index)
    {
        if (index < 0 || index >= tabMusics.Length)
        {
            Debug.LogWarning("Index de musique invalide !");
            return;
        }
        musicIndex = index;
        audioSource.clip = tabMusics[musicIndex];
        audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            lastPosition = audioSource.time;  // Sauvegarde la position de lecture actuelle
            audioSource.Pause();              // Met en pause la musique
        }
    }

    public void ResumeMusic()
    {
        if (!audioSource.isPlaying && lastPosition > 0)
        {
            audioSource.time = lastPosition;  // Reprend à la position sauvegardée
            audioSource.Play();               // Relance la musique
        }
        //Si on ne peut pas reprendre, on continu de jouer une autre musique
        else
            PlayNextSong();
    }

    //Cette fonction permet de jouer un effet sonnor
    //Les autre classe on juste à l'appeler et à renseigner en param l'indice de l'effet sonnor placé dans l'AudioManager
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
        Destroy(objectTempo, tabSoundEffect[index].length); // on détruit l'objet une fois qu'on arrive a la fin du clip audio de l'objet
        return audioSource;
    }

}
