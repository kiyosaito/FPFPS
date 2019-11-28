using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public Button gameplayButton;
    public Button graphicsButton;
    public Button audioButton;
    public Button controlsButton;
    public Button backButton;
    #endregion
    #region Screenmode
    public TMPro.TMP_Dropdown screenModeDropdown;
    public FullScreenMode[] screenMode = new FullScreenMode[3];
    #endregion

    public Toggle timerToggle;
    public Toggle splitToggle;
    public Toggle segmentToggle;
    public Toggle comparisonToggle;
    private int sceneToContinue;

    public Slider timeScale;
    public TMP_Dropdown difficultySelect;

    public Toggle ggf;
    public Toggle speedrun;

    public Slider mouseSensitivity;

    public Slider masterVolume;
    public Slider sfxVolume;
    public Slider musicVolume;

    public TMP_Dropdown qualityLevel;
    #endregion

    private void Start()
    {
        #region Resoultion
        GameManager.Instance.LoadSavedOptions();

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

        if (-1 == GameManager.Instance.GameResolutionIdx)
        {
            GameManager.Instance.GameResolutionIdx = curResIndex;
        }
        else
        {
            curResIndex = GameManager.Instance.GameResolutionIdx;
        }

        resDropdown.value = curResIndex;
        resDropdown.RefreshShownValue();

        #endregion

        SetupMenu();
    }

    private void SetupMenu()
    {
        GameManager.Instance.LoadSavedOptions();
        InputManager.Instance.LoadSavedOptions();

        OptionsMenuTimeScale(GameManager.Instance.GameTimeScale);
        SetDifficulty((int)GameManager.Instance.Difficulty);
        DisplayTimer(GameManager.Instance.ShowTimer);
        GottaGoFast(GameManager.Instance.GottaGoFast);
        Speedrun(GameManager.Instance.SpeedrunMode);
        SplitInfo(GameManager.Instance.ShowSplits);
        Segment(GameManager.Instance.ShowSegmentTime);
        Comparison(GameManager.Instance.ShowTotalTimeComparison);
        OptionsMenuSetQuality(GameManager.Instance.QualityLevelIndex);
        OptionsMenuSetResolution(GameManager.Instance.GameResolutionIdx);
        OptionsMenuScreenMode(GameManager.Instance.ScreenMode);
        OptionsMenuSetMainVolume(GameManager.Instance.MasterVolume);
        OptionsMenuSetSFXVolume(GameManager.Instance.SFXVolume);
        OptionsMenuSetMusicVolume(GameManager.Instance.MusicVolume);
        OptionsMenuSetMouseSensitivy(InputManager.Instance.MouseSensitivity);
    }

    #region Options Menu
    #region Gameplay
    public void OptionsMenuTimeScale(float scale)
    {
        if ((null != timeScale) && (timeScale.value != scale))
        {
            timeScale.value = scale;
        }
        GameManager.Instance.GameTimeScale = scale;
    }
    public void SetDifficulty(int difficultyIndex)
    {
        if ((null != difficultySelect) && (difficultySelect.value != difficultyIndex))
        {
            difficultySelect.value = difficultyIndex;
        }
        GameManager.Instance.Difficulty = (GameManager.TimerDifficultySetting)(difficultyIndex);
    }
    public void DisplayTimer(bool exist)
    {
        if ((null != timerToggle) && (timerToggle.isOn != exist))
        {
            timerToggle.isOn = exist;
        }
        GameManager.Instance.ShowTimer = exist;
        if ((null != splitToggle) && (null != segmentToggle) && (null != comparisonToggle))
        {
            splitToggle.gameObject.SetActive(exist);
            segmentToggle.gameObject.SetActive(exist);
            comparisonToggle.gameObject.SetActive(exist);
        }
    }
    public void GottaGoFast(bool exist)
    {
        if ((null != ggf) && (ggf.isOn != exist))
        {
            ggf.isOn = exist;
        }
        GameManager.Instance.GottaGoFast = exist;
    }
    public void Speedrun(bool exist)
    {
        if ((null != speedrun) && (speedrun.isOn != exist))
        {
            speedrun.isOn = exist;
        }
        GameManager.Instance.SpeedrunMode = exist;
    }
    public void SplitInfo(bool exist)
    {
        if ((null != splitToggle) && (splitToggle.isOn != exist))
        {
            splitToggle.isOn = exist;
        }
        GameManager.Instance.ShowSplits = exist;
    }
    public void Segment(bool exist)
    {
        if ((null != segmentToggle) && (segmentToggle.isOn != exist))
        {
            segmentToggle.isOn = exist;
        }
        GameManager.Instance.ShowSegmentTime = exist;
    }
    public void Comparison(bool exist)
    {
        if ((null != comparisonToggle) && (comparisonToggle.isOn != exist))
        {
            comparisonToggle.isOn = exist;
        }
        GameManager.Instance.ShowTotalTimeComparison = exist;
    }
    #endregion
    #region Graphics

    public void OptionsMenuSetQuality(int qualityIndex)
    {
        GameManager.Instance.QualityLevelIndex = qualityIndex;
        if ((null != qualityLevel) && (qualityLevel.value != qualityIndex))
        {
            qualityLevel.value = qualityIndex;
        }
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void OptionsMenuSetResolution(int resIndex)
    {
        Resolution resolution = res[resIndex];
        GameManager.Instance.GameResolutionIdx = resIndex;
        if ((null != resDropdown) && (resDropdown.value != resIndex))
        {
            resDropdown.value = resIndex;
            resDropdown.RefreshShownValue();
        }
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void OptionsMenuScreenMode(int idx)
    {
        GameManager.Instance.ScreenMode = idx;
        if ((null != screenModeDropdown) && (screenModeDropdown.value != idx))
        {
            screenModeDropdown.value = idx;
        }
        Screen.fullScreenMode = screenMode[screenModeDropdown.value];
    }
    #endregion
    #region Audio
    public void OptionsMenuSetMainVolume(float volume)
    {
        GameManager.Instance.MasterVolume = volume;
        if ((null != masterVolume) && (masterVolume.value != volume))
        {
            masterVolume.value = volume;
        }
        master.audioMixer.SetFloat("MasterVol", volume);
    }
    public void OptionsMenuSetSFXVolume(float volume)
    {
        GameManager.Instance.SFXVolume = volume;
        if ((null != sfxVolume) && (sfxVolume.value != volume))
        {
            sfxVolume.value = volume;
        }
        sFX.audioMixer.SetFloat("SFXVol", volume);
    }
    public void OptionsMenuSetMusicVolume(float volume)
    {
        GameManager.Instance.MusicVolume = volume;
        if ((null != musicVolume) && (musicVolume.value != volume))
        {
            musicVolume.value = volume;
        }
        music.audioMixer.SetFloat("MusicVol", volume);
    }
    #endregion
    #region Controls
    public void OptionsMenuSetMouseSensitivy(float sensitivity)
    {
        if ((null != mouseSensitivity) && (mouseSensitivity.value != sensitivity))
        {
            mouseSensitivity.value = sensitivity;
        }
        InputManager.Instance.MouseSensitivity = sensitivity;
    }

    public void ButtonIsEnabled(bool buttonEnabled)
    {
        gameplayButton.interactable = buttonEnabled;
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
        GameManager.Instance.StartNewGame();
    }

    public void MainMenuLoadLevel()
    {
        GameManager.Instance.ContinueGame();
    }

    public void MainMenuQuit()
    {
        Application.Quit();
    }

    public void SaveSettings()
    {
        GameManager.Instance.SaveOptions();
        InputManager.Instance.SaveOptions();
    }
    #endregion
}
