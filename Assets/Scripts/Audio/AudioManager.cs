using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : UnitySingleton<AudioManager>
{
    #region Varibles 
    private GameObject musicManager, sFXManager;
    public AudioSource musicSource, sFXSource;
    public AudioClip menuMusicV1, menuMusicV2;
    public AudioClip[] musicClips;
    public GameObject canvasV1, canvasV2;
    #endregion



    private void Start()
    {
        musicManager = GameObject.Find("Music Source");
        musicSource = musicManager.GetComponent<AudioSource>();
        sFXManager = GameObject.Find("SFX Source");
        sFXSource = sFXManager.GetComponent<AudioSource>();
    }
    private void Update()
    {
        MainMenuMusic();
    }

    void MainMenuMusic()
    {
        if ((GameManager.GameScene.MainMenu == GameManager.Instance.GetCurrentScene()) || (GameManager.GameScene.LevelSelect == GameManager.Instance.GetCurrentScene()))
        {
            if (GameManager.Instance.FirstLevelReached == false)
            {
                musicSource.clip = menuMusicV1;
                musicSource.loop = true;
                musicSource.Play();
            }
            else
            {
                musicSource.clip = menuMusicV2;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
    }
    void GameMusic()
    {
        if ((GameManager.GameScene.MainMenu != GameManager.Instance.GetCurrentScene()) || (GameManager.GameScene.LevelSelect != GameManager.Instance.GetCurrentScene()))
        {
            StartCoroutine(PlayAudioSequentially());
        }
    }

    #region 
    IEnumerator PlayAudioSequentially()
    {
        yield return null;
        musicSource.loop = false;

        for (int i = 0; i < musicClips.Length; i++) // Loops through the array of clips to the source
        {
            musicSource.clip = musicClips[i];

            musicSource.Play(); // Play the clips

            while (musicSource.isPlaying)
            {
                yield return null;
            }
        } // Go back and select the next clip in the array
    }
    #endregion



}