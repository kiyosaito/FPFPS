using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : UnitySingleton<MusicManager>
{
    #region Varibles 
    public AudioSource musicSource; // Declaring the audio source is public
    public AudioClip[] audiotClips;
    public AudioClip[] audioClips2; // An Array of music clips
    public GameObject canvas;
    #endregion

    #region Start Coroutine
    public void Awake()
    {
        if (canvas.activeSelf == true)
        {
            StartCoroutine(PlayAudioV1Sequentially());
        }
        else
        {
            StartCoroutine(PlayAudioV2Sequentially());
        }

    }
    #endregion
    private void Start()
    {
        musicSource = FindObjectOfType<AudioSource>();
        
    }
    #region 
    IEnumerator PlayAudioV1Sequentially()
    {
        yield return null;

        for (int i = 0; i < audioClips2.Length; i++) // Loops through the array of clips to the source
        {
            musicSource.clip = audioClips2[i];

            musicSource.Play(); // Play the clips

            while (musicSource.isPlaying)
            {
                yield return null;
            }
        } // Go back and select the next clip in the array
    }
    IEnumerator PlayAudioV2Sequentially()
    {
        yield return null;

        for (int i = 0; i < audioClips2.Length; i++) // Loops through the array of clips to the source
        {
            musicSource.clip = audioClips2[i];

            musicSource.Play(); // Play the clips

            while (musicSource.isPlaying)
            {
                yield return null;
            }
        } // Go back and select the next clip in the array
    }
    #endregion



}