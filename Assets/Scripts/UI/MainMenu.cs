﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region References/Variables
    #region Audio
    public AudioMixerGroup master;
    public AudioMixerGroup sFX;
    public AudioMixerGroup music;
    #endregion
    #region Resolution
    public TMPro.TMP_Dropdown resDropdown;
    private Resolution[] res;
    #endregion
    #region OptionsButtons
    public Button graphicsButton;
    public Button audioButton;
    public Button controlsButton;
    public Button backButton;
    #endregion
    #region Screenmode
    public TMPro.TMP_Dropdown screenModeDropdown;
    public FullScreenMode[] screenMode = new FullScreenMode[3];
    #endregion

    private int sceneToContinue;
    #endregion

    private void Start()
    {
        #region Resoultion
        res = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> resOptions = new List<string>();

        int curResIndex = 0;


        for (int i = 0; i < res.Length; i++)
        {
            string option = res[i].width + "x" + res[i].height;
            resOptions.Add(option);
            if (res[i].width == Screen.currentResolution.width && res[i].height == Screen.currentResolution.height)
            {
                curResIndex = i;
            }
        }
        resDropdown.AddOptions(resOptions);
        resDropdown.value = curResIndex;
        resDropdown.RefreshShownValue();
        #endregion
        
    }

    #region Options Menu
    #region Graphics

    public void OptionsMenuSetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
   
    public void OptionsMenuSetResolution(int resIndex)
    {
        Resolution resolution = res[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void OptionsMenuScreenMode( )
    {
        Screen.fullScreenMode = screenMode[screenModeDropdown.value];
    }
    #endregion
    #region Audio
    public void OptionsMenuSetMainVolume(float volume)
    {
        master.audioMixer.SetFloat("Volume", volume);
    }
    public void OptionsMenuSetSFXVolume(float volume)
    {
        sFX.audioMixer.SetFloat("Volume", volume);
    }
    public void OptionsMenuSetMusicVolume(float volume)
    {
        music.audioMixer.SetFloat("Volume", volume);
    }
    #endregion
    #region Controls
    public void OptionsMenuSetMouseSensitivy(float sensitivity)
    {
        Debug.Log("new sensitivity " + sensitivity.ToString());
        InputManager.Instance.MouseSensitivity = sensitivity;
    }

    public void ButtonIsEnabled(bool buttonEnabled)
    {
        graphicsButton.interactable = buttonEnabled;
        audioButton.interactable = buttonEnabled;
        controlsButton.interactable = buttonEnabled;
        backButton.interactable = buttonEnabled;
    }
    
    #endregion
    #endregion

    #region Main Menu
    public void MainMenuNewGame()
    {
        SceneManager.LoadScene(1);
        //SaveLoad.ResetSaves();
    }

    public void MainMenuLoadLevel()
    {
        sceneToContinue = PlayerPrefs.GetInt("SavedScene");
        if (sceneToContinue != 0)
        {
            SceneManager.LoadScene(sceneToContinue);
        }
        else
        {
            return;
        }
    }

    public void MainMenuQuit()
    {
        Application.Quit();
    }
    #endregion
}
