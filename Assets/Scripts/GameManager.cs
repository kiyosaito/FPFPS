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
        _sceneNames.Add(GameScene.EndMenu, "Victory");
        _sceneNames.Add(GameScene.Tutorial_1, "Tutorial 1");
        _sceneNames.Add(GameScene.Level_1, "Stage 1");
        _sceneNames.Add(GameScene.Level_2, "Stage 2");
        _sceneNames.Add(GameScene.Level_3, "Stage 3");
    }

    #endregion

    #region OptionsData

    private class OptionsData : PersistableData
    {
        public const int CurrentFormatVersion = 2;

        public float FileVersion = 0;

        public int ScreenMode = 2;
        public int ResolutionIdx = -1;
        public int QualityLevelIndex = 5;
        public float MasterVolume = 0f;
        public float SFXVolume = 0f;
        public float MusicVolume = 0f;
        public float GameTimeScale = 1f;
        public bool GottaGoFast = false;
        public bool SpeedrunMode = false;
        public int TimerDifficultySetting = (int)(GameManager.TimerDifficultySetting.Forte);
        public bool ShowTimer = true;
        public bool ShowSplits = false;
        public bool ShowSegmentTime = false;
        public bool ShowTotalTimeComparison = false;
        public int LevelReached = -1;
        public int CurrentLevelProgress = -1;

        public override void Save(GameDataWriter writer)
        {
            writer.Write(-CurrentFormatVersion);

            writer.Write(ScreenMode);
            writer.Write(ResolutionIdx);
            writer.Write(QualityLevelIndex);
            writer.Write(MasterVolume);
            writer.Write(SFXVolume);
            writer.Write(MusicVolume);
            writer.Write(GameTimeScale);
            writer.Write(GottaGoFast);
            writer.Write(SpeedrunMode);
            writer.Write(TimerDifficultySetting);
            writer.Write(ShowTimer);
            writer.Write(ShowSplits);
            writer.Write(ShowSegmentTime);
            writer.Write(ShowTotalTimeComparison);
            writer.Write(LevelReached);
            writer.Write(CurrentLevelProgress);
        }

        protected override void Init(GameDataReader reader)
        {
            FileVersion = -reader.ReadInt();

            if (CurrentFormatVersion == FileVersion)
            {
                ScreenMode = reader.ReadInt();
                ResolutionIdx = reader.ReadInt();
                QualityLevelIndex = reader.ReadInt();
                MasterVolume = reader.ReadFloat();
                SFXVolume = reader.ReadFloat();
                MusicVolume = reader.ReadFloat();
                GameTimeScale = reader.ReadFloat();
                GottaGoFast = reader.ReadBool();
                SpeedrunMode = reader.ReadBool();
                TimerDifficultySetting = reader.ReadInt();
                ShowTimer = reader.ReadBool();
                ShowSplits = reader.ReadBool();
                ShowSegmentTime = reader.ReadBool();
                ShowTotalTimeComparison = reader.ReadBool();
                LevelReached = reader.ReadInt();
                CurrentLevelProgress = reader.ReadInt();
            }
        }

        public OptionsData() { }
    }

    private void SaveData()
    {
        OptionsData data = new OptionsData();

        data.ScreenMode = _screenMode;
        data.ResolutionIdx = _resolutionIdx;
        data.QualityLevelIndex = _qualityLevelIndex;
        data.MasterVolume = _masterVolume;
        data.SFXVolume = _sFXVolume;
        data.MusicVolume = _musicVolume;
        data.GameTimeScale = _gameTimeScale;
        data.GottaGoFast = _gottaGoFast;
        data.SpeedrunMode = _speedrunMode;
        data.TimerDifficultySetting = (int)(_timerDifficultySetting);
        data.ShowTimer = _showTimer;
        data.ShowSplits = _showSplits;
        data.ShowSegmentTime = _showSegmentTime;
        data.ShowTotalTimeComparison = _showTotalTimeComparison;
        data.LevelReached = _levelReached;
        data.CurrentLevelProgress = _currentLevelProgress;

        GameDataWriter.SaveData("Settings", data);
    }

    private void LoadData()
    {
        OptionsData data = GameDataReader.LoadData<OptionsData>("Settings");

        if ((null != data) && (OptionsData.CurrentFormatVersion == data.FileVersion))
        {
            _screenMode = data.ScreenMode;
            _resolutionIdx = data.ResolutionIdx;
            _qualityLevelIndex = data.QualityLevelIndex;
            _masterVolume = data.MasterVolume;
            _sFXVolume = data.SFXVolume;
            _musicVolume = data.MusicVolume;
            _gameTimeScale = data.GameTimeScale;
            _gottaGoFast = data.GottaGoFast;
            _speedrunMode = data.SpeedrunMode;
            _timerDifficultySetting = (TimerDifficultySetting)(data.TimerDifficultySetting);
            _showTimer = data.ShowTimer;
            _showSplits = data.ShowSplits;
            _showSegmentTime = data.ShowSegmentTime;
            _showTotalTimeComparison = data.ShowTotalTimeComparison;
            _levelReached = data.LevelReached;
            _currentLevelProgress = data.CurrentLevelProgress;
        }
    }

    #endregion

    #region Private Variables

    private bool _optionsLoaded = false;

    private Dictionary<GameScene, string> _sceneNames = new Dictionary<GameScene, string>();

    private int _screenMode = 2;
    private int _resolutionIdx = -1;
    private int _qualityLevelIndex = 5;
    private float _masterVolume = 0f;
    private float _sFXVolume = 0f;
    private float _musicVolume = 0f;
    private float _gameTimeScale = 1f;
    private bool _gottaGoFast = false;
    private bool _speedrunMode = false;
    private TimerDifficultySetting _timerDifficultySetting = TimerDifficultySetting.Forte;
    private bool _showTimer = true;
    private bool _showSplits = false;
    private bool _showSegmentTime = false;
    private bool _showTotalTimeComparison = false;

    [SerializeField]
    private int _levelReached = -1;

    [SerializeField]
    private int _currentLevelProgress = -1;

    private List<GameScene> _levelProgression = new List<GameScene>
        ( new GameScene[5] {
            GameScene.Tutorial_1,
            GameScene.Level_1,
            GameScene.Level_2,
            GameScene.Level_3,
            GameScene.EndMenu
        });

    private bool _levelSelectMode = false;

    private bool _initialised = false;

    #endregion

    #region Public Properties

    public enum GameScene
    {
        Tutorial_1,
        Level_1,
        Level_2,
        Level_3,
        Unknown,
        MainMenu,
        LevelSelect,
        EndMenu,
    }

    public bool FirstLevelReached { get { return (-1 != _levelReached); } }

    public int ScreenMode
    {
        get { return _screenMode; }
        set { _screenMode = value; }
    }

    public int GameResolutionIdx
    {
        get { return _resolutionIdx; }
        set { _resolutionIdx = value; }
    }

    public float MasterVolume
    {
        get { return _masterVolume; }
        set { _masterVolume = value; }
    }

    public int QualityLevelIndex
    {
        get { return _qualityLevelIndex; }
        set { _qualityLevelIndex = value; }
    }

    public float SFXVolume
    {
        get { return _sFXVolume; }
        set { _sFXVolume = value; }
    }

    public float MusicVolume
    {
        get { return _musicVolume; }
        set { _musicVolume = value; }
    }

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

    public bool Initialised
    {
        get { return _initialised; }
    }

    #endregion

    #region Public Functions

    public void LoadSavedOptions()
    {
        if (!_optionsLoaded)
        {
            _optionsLoaded = true;
            LoadData();
        }
    }

    public void SaveOptions()
    {
        SaveData();
    }

    public void SelectLevel(GameScene level)
    {
        _levelSelectMode = true;

        LoadLevel(level);
    }

    public void StartNewGame()
    {
        _levelSelectMode = false;

        _currentLevelProgress = 0;
        SaveOptions();

        LoadLevel(_levelProgression[_currentLevelProgress]);
    }

    public void ContinueGame()
    {
        _levelSelectMode = false;

        if (-1 == _currentLevelProgress)
        {
            ++_currentLevelProgress;
            _levelReached = Mathf.Max(_levelReached, _currentLevelProgress);
            SaveOptions();
        }

        LoadLevel(_levelProgression[_currentLevelProgress]);
    }

    public void LevelFinished()
    {
        if (!_levelSelectMode)
        {
            ++_currentLevelProgress;
            _levelReached = Mathf.Max(_levelReached, _currentLevelProgress);
            SaveOptions();
        }
    }

    public void LoadNextLevel()
    {
        if (_levelSelectMode)
        {
            Time.timeScale = 1f;
            SceneManagerLoadScene(GameScene.LevelSelect);
        }
        else
        {
            LoadLevel(_levelProgression[_currentLevelProgress]);
        }
    }

    public void RestartLevel()
    {
        GameScene currentLevel = GetCurrentScene();

        if (_levelProgression.Contains(currentLevel))
        {
            LoadLevel(currentLevel);
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
        SceneManagerLoadScene(GameScene.MainMenu);
    }

    public void GoToLevelSelectMenu()
    {
        Time.timeScale = 1f;
        SceneManagerLoadScene(GameScene.LevelSelect);
    }

    private void Update()
    {
        GameScene current = GetCurrentScene();
        if ((GameScene.LevelSelect == current) || (GameScene.MainMenu == current))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
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

        SceneManagerLoadScene(level);
    }

    private void InitialiseLevel(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= InitialiseLevel;

        _initialised = true;
        Time.timeScale = _gameTimeScale;
        CheckPointManager.Instance.Init();
        TimerManager.Instance.StartTimers();

        TimerMenu timerMenu = FindObjectOfType<TimerMenu>();
        if (null != timerMenu)
        {
            timerMenu.Init();
        }
    }

    private void SceneManagerLoadScene(GameManager.GameScene nextScene)
    {
        SceneManager.LoadScene(_sceneNames[nextScene]);

        if (null != AudioManager.Instance)
        {
            // Inform Audio Manager
            AudioManager.Instance.SceneChanged(nextScene);
        }
    }

    #endregion
}
