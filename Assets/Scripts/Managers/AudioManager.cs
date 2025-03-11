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

public enum AudioType
{
    UIButton, //0
    BruitDePasTerre, //1
    BruitDePasMetal, //2
    BruitDePasBois, //3
    BruitDePasBeton, //4
    Camion, //5
    CandyCrushAlert, //6
    CandyCrushMatch, //7
    CandyCrushSuperMatch, //8
    Pioche, //9
    Pioche1, //10
    Pioche2, //11
    Tronceneuse, //12
    Corbeau, //13
    Poule1, //14
    Poule2, //15
    Deffaite, //16
    Victory, //17
    UsineAssemblyCompletedCircuit, //18
    UsineAssemblyCompletedOnComposant, //19
    MineGame1PlayerTakeObject, //20
    Chat1,//21
    Chat2//22
}


public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Clips Music")]
    //Chaque Tuple contient 2 AudioClips, le premier est la musique de d�but, le deuxi�me est la musique de boucle
    public List<CTuple<AudioClip,AudioClip>> tabMusic;


    [Header("Audio Clips Sound Effect")]
    public AudioClip[] tabSoundEffect;

    private Dictionary<AudioType, AudioClip> DicoAudioClips = new Dictionary<AudioType, AudioClip>();

    [Header("Audio Sources")]
    public AudioSource audioSource; //responsable de jouer les musiques de fond
    public AudioMixerGroup soundEffectMixer;//Mixer pour les effets sonores

    //pour suivre la musique qui est jouer
    private int musicStateIndex = -1;
    private int musicToPlay = 0;


    void Start()
    {
        GameProgressManager.Instance.OnGameProgressStateChange.AddListener(HandleGameProgressStateChanged);

        foreach (AudioType audioType in System.Enum.GetValues(typeof(AudioType)))
        {
            DicoAudioClips.Add(audioType, tabSoundEffect[(int)audioType]);
        }
        musicStateIndex = 0;
    }

    //Il faudrait qu'en fonction du GameProgressState, on joue une musique diff�rente, en premier la musique de d�but, puis la musique de boucle en boucle
    private void Update()
    {
        if (audioSource.isPlaying == false)
        {
            print(musicStateIndex);
            print(tabMusic.Count);
            if (musicStateIndex >= 0 && musicStateIndex < tabMusic.Count)
            {
                print("2");
                if (musicToPlay == 0)
                {
                    print("3");
                    audioSource.clip = tabMusic[musicStateIndex].Item1;
                    audioSource.Play();
                    musicToPlay = 1;
                } else {
                    print("4");
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


    public AudioClip GetClip(AudioType at)
    {
        DicoAudioClips.TryGetValue(at, out AudioClip clip);
        return clip;
    }

    //Cette fonction permet de jouer un effet sonnor
    //Les autre classe on juste � l'appeler et � renseigner en param l'indice de l'effet sonnor plac� dans l'AudioManager
    public AudioSource PlaySoundEffet(AudioType at)
    {
        //en fonction de l'audiotype et on jou le son correspondant dans le dico
        DicoAudioClips.TryGetValue(at, out AudioClip clip);
        if (clip == null)
        {
            Debug.LogError("AudioClip not found for AudioType : " + at);
            return null;
        }

        GameObject objectTempo = new GameObject("TempAudio");
        AudioSource audioSource = objectTempo.AddComponent<AudioSource>();//on ajoute un audio source au game object
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = soundEffectMixer; // on ajoute le mixer de l'audio source
        audioSource.Play();
        Destroy(objectTempo, clip.length); // on d�truit l'objet une fois qu'on arrive a la fin du clip audio de l'objet
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
