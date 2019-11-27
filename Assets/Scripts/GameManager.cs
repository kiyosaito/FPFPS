using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    #region Function Overrides

    protected override void Setup()
    {
        _sceneNames.Add(GameScene.MainMenu, "MainMenu");
        _sceneNames.Add(GameScene.LevelSelect, "LevelSelect");
        _sceneNames.Add(GameScene.Level_1, "Stage 1");
        _sceneNames.Add(GameScene.Level_2, "Stage 2");
        _sceneNames.Add(GameScene.Level_3, "Stage 3");

        // TODO: Load timer related settings, and level progression
    }

    #endregion

    #region Private Variables

    private Dictionary<GameScene, string> _sceneNames = new Dictionary<GameScene, string>();

    [SerializeField]
    private float _gameTimeScale = 1f;

    [SerializeField]
    private bool _gottaGoFast = false;

    [SerializeField]
    private bool _speedrunMode = false;

    [SerializeField]
    private TimerDifficultySetting _timerDifficultySetting = TimerDifficultySetting.Forte;

    [SerializeField]
    private bool _showTimer = true;

    [SerializeField]
    private bool _showSplits = false;

    [SerializeField]
    private bool _showSegmentTime = false;

    [SerializeField]
    private bool _showTotalTimeComparison = false;

    [SerializeField]
    private int _levelReached = -1;

    [SerializeField]
    private int _currentLevelProgress = -1;

    [SerializeField]
    private GameScene _currentLevel = GameScene.MainMenu;

    private List<GameScene> _levelProgression = new List<GameScene>(new GameScene[3] {GameScene.Level_1, GameScene.Level_2, GameScene.Level_3});

    private bool _levelSelectMode = false;

    private bool _initialised = false;

    #endregion

    #region Public Properties

    public enum GameScene
    {
        Level_1,
        Level_2,
        Level_3,
        Unknown,
        MainMenu,
        LevelSelect,
    }

    public bool FirstLevelReached { get { return (-1 != _levelReached); } }

    public float GameTimeScale
    {
        get { return _gameTimeScale; }
        set { _gameTimeScale = value; }
    }

    public bool GottaGoFast
    {
        get { return _gottaGoFast; }
        set { _gottaGoFast = value; }
    }

    public bool SpeedrunMode
    {
        get { return _speedrunMode; }
        set { _speedrunMode = value; }
    }

    public enum TimerDifficultySetting
    {
        Fortissimo, // Hard     // Reaching a checkpoint sets remaining time to checkpoint time
        Forte,      // Normal   // Reaching a checkpoint adds checkpoint time to remaining time, extra time can be lost with deaths
        Piano,      // Medium   // Reaching a checkpoint adds checkpoint time to remaining time, gained time is retained on death
        Pianissimo, // Easy     // There is no remaining time
    }

    public TimerDifficultySetting Difficulty
    {
        get { return _timerDifficultySetting; }
        set { _timerDifficultySetting = value; }
    }

    public bool ShowTimer
    {
        get { return _showTimer; }
        set { _showTimer = value; }
    }

    public bool ShowSplits
    {
        get { return _showSplits; }
        set { _showSplits = value; }
    }

    public bool ShowSegmentTime
    {
        get { return _showSegmentTime; }
        set { _showSegmentTime = value; }
    }

    public bool ShowTotalTimeComparison
    {
        get { return _showTotalTimeComparison; }
        set { _showTotalTimeComparison = value; }
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

        _currentLevel = level;

        LoadLevel(level);
    }

    public void StartNewGame()
    {
        _levelSelectMode = false;

        _currentLevelProgress = 0;
        _currentLevel = _levelProgression[_currentLevelProgress];

        LoadLevel(_currentLevel);
    }

    public void ContinueGame()
    {
        _levelSelectMode = false;

        if (-1 == _currentLevelProgress)
        {
            ++_currentLevelProgress;
            ++_levelReached;
        }

        _currentLevel = _levelProgression[_currentLevelProgress];

        LoadLevel(_currentLevel);
    }

    public void LevelFinished()
    {
        if (_levelSelectMode)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_sceneNames[GameScene.LevelSelect]);
        }
        else
        {
            ++_currentLevelProgress;
            _levelReached = Mathf.Max(_levelReached, _currentLevelProgress);

            _currentLevel = _levelProgression[_currentLevelProgress];

            LoadLevel(_currentLevel);
        }
    }

    public void RestartLevel()
    {
        if (_levelProgression.Contains(_currentLevel))
        {
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

    public void BackToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_sceneNames[GameScene.MainMenu]);
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
        Time.timeScale = _gameTimeScale;
        CheckPointManager.Instance.Init();
        TimerManager.Instance.StartTimers();
    }

    #endregion
}
