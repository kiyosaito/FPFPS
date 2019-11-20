﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    #region Function Overrides

    protected override void Setup()
    {
        _sceneNames.Add(GameScene.MainMenu, "MainMenu");
        _sceneNames.Add(GameScene.LevelSelect, "....");
        _sceneNames.Add(GameScene.Level_1, "Stage 1");
        _sceneNames.Add(GameScene.Level_2, "Stage 2");
        _sceneNames.Add(GameScene.Level_3, "Stage 3");

        // TODO: Load timer related settings, and level progression
    }

    #endregion

    #region Private Variables

    private Dictionary<GameScene, string> _sceneNames = new Dictionary<GameScene, string>();

    private TimerDifficultySetting _timerDifficultySetting = TimerDifficultySetting.Normal;

    private bool _showTotalTime = true;

    private bool _showSplits = false;

    private bool _showSegmentTime = false;

    private GameScene _levelReached = GameScene.MainMenu;

    private GameScene _currentLevel = GameScene.MainMenu;

    private bool _levelSelectMode = false;

    private bool _initialised = false;

    #endregion

    #region Public Properties

    public enum GameScene
    {
        Unknown,
        MainMenu,
        LevelSelect,
        Level_1 = 1,
        Level_2 = 2,
        Level_3 = 3,
    }

    public enum TimerDifficultySetting
    {
        Hard,   // Fortissimo   // Reaching a checkpoint sets remaining time to checkpoint time
        Normal, // Forte        // Reaching a checkpoint adds checkpoint time to remaining time, extra time can be lost with deaths
        Medium, // Piano        // Reaching a checkpoint adds checkpoint time to remaining time, gained time is retained on death
        Easy,   // Pianissimo   // There is no remaining time
    }

    public bool ShowTotalTime
    {
        get { return _showTotalTime; }
    }

    public bool ShowSplits
    {
        get { return _showSplits; }
    }

    public bool ShowSegmentTime
    {
        get { return _showSegmentTime; }
    }

    public GameScene CurrentLevel
    {
        get { return _currentLevel; }
    }

    public bool Initialised
    {
        get { return _initialised; }
    }

    #endregion

    #region Public Functions

    public void SelectLevel(GameScene level)
    {
        _levelSelectMode = true;

        LoadLevel(level);
    }

    public void ContinueGame()
    {
        _levelSelectMode = false;

        if (0 == _currentLevel)
        {
            ++_currentLevel;
        }

        LoadLevel(_currentLevel);
    }

    public void LevelFinished()
    {
        if (_levelSelectMode)
        {
            // TODO: Load level select menu/scene
        }
        else
        {
            _currentLevel = (GameScene)(((int)_currentLevel) + 1);

            LoadLevel(_currentLevel);
        }
    }

    public GameScene GetCurrentScene()
    {
        GameScene currentScene = GameScene.Unknown;
        string currentSceneName = SceneManager.GetActiveScene().name;

        foreach (var pair in _sceneNames)
        {
            if (pair.Value.Equals(currentSceneName))
            {
                currentScene = pair.Key;
                break;
            }
        }

        return currentScene;
    }

    #endregion

    #region Private Functions

    private void LoadLevel(GameScene level)
    {
        if (_sceneNames.ContainsKey(level))
        {
            StartCoroutine(LoadSceneRoutine(level));
        }
        else
        {
            Debug.LogError("Scene name not set for GameScene " + level.ToString());
        }
    }

    private IEnumerator LoadSceneRoutine(GameScene level)
    {
        yield return new WaitForEndOfFrame();

        SceneManager.sceneLoaded += InitialiseLevel;

        SceneManager.LoadScene(_sceneNames[level]);
    }

    private void InitialiseLevel(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= InitialiseLevel;

        _initialised = true;
        CheckPointManager.Instance.Init();
        TimerManager.Instance.StartTimers();
    }

    #endregion
}
