using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : UnitySingleton<AudioManager>
{
    #region Varibles 
    private GameObject musicManager;
    public AudioSource musicSource;
    public AudioClip menuMusicV1, menuMusicV2;
    public AudioClip level1, level2, level3;
    public AudioClip[] musicClips;
    public GameManager.GameScene lastScene = GameManager.GameScene.Unknown;

    #endregion

    private void Start()
    {
        SceneChanged();
    }

    public void SceneChanged()
    {
        if (lastScene != GameManager.Instance.GetCurrentScene())
        {
            MainMenuMusic();
            GameMusic();
        }

        lastScene = GameManager.Instance.GetCurrentScene();
    }

    void MainMenuMusic()
    {
        if ((GameManager.GameScene.MainMenu == GameManager.Instance.GetCurrentScene()) || (GameManager.GameScene.LevelSelect == GameManager.Instance.GetCurrentScene()))
        {
            // Change after menu V1 is in game
            if (false && (GameManager.Instance.FirstLevelReached == false))
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
        //if ((GameManager.GameScene.MainMenu != GameManager.Instance.GetCurrentScene()) || (GameManager.GameScene.LevelSelect != GameManager.Instance.GetCurrentScene()))
        //{
        //    StartCoroutine(PlayAudioSequentially());
        //}

        if (GameManager.GameScene.Level_1 == GameManager.Instance.GetCurrentScene())
        {
            musicSource.clip = level1;
            musicSource.loop = true;
            musicSource.Play();
        }
        if (GameManager.GameScene.Level_2 == GameManager.Instance.GetCurrentScene())
        {
            musicSource.clip = level2;
            musicSource.loop = true;
            musicSource.Play();
        }
        if (GameManager.GameScene.Level_3 == GameManager.Instance.GetCurrentScene())
        {
            musicSource.clip = level3;
            musicSource.loop = true;
            musicSource.Play();
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