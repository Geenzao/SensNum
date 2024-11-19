using UnityEngine;
using System.Collections;


/*
 * Avant chaque mini jeux, il y a une cin�matique d'explication. 
 * Pour chaque cin�matique, il y a un ensemble de CinematicClip qui sont g�r� par le CinematicManager
 * 
 * Cette classe CinematicManager est responssable de jouer les cinematique 
 * et de coordon� les dialogue dans le future qui seront afficher dessus
 *
 */

public class CinematicManager : MonoBehaviour
{
    public CinematicClip[] clips; // Liste de clips cin�matiques
    private float TimeBeforNextClip = 4.0f;

    private void Start()
    {
        //Au d�but on cache toute les cinematic clip au cas ou
        foreach (CinematicClip clip in clips)
        {
            clip.isRening = false;
            clip.gameObject.SetActive(false);
        }

            StartCoroutine(PlayCinematics());
    }

    private IEnumerator PlayCinematics()
    {
        //On lance les cinematiques pendant 3 seconde � la suite
        foreach (CinematicClip clip in clips)
        {
            Debug.Log($"Playing clip: {clip.gameObject.name}");

            //On active la cinematic clip
            clip.gameObject.SetActive( true );
            clip.isRening = true;

            yield return new WaitForSeconds(TimeBeforNextClip);

            //On d�sactive la cinematic clip
            clip.gameObject.SetActive(false);
            clip.isRening = false;
        }
        Debug.Log("All cinematics played.");
    }
}
