using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : UnitySingleton<TimerManager>
{
    #region TimerData

    private class TimerData : PersistableData
    {
        public const int CurrentFormatVersion = 1;

        public float FileVersion = 0;
        public float PBTotalTime = 0f;
        public float[] PBSplitTimes = null;
        public float[] PBSegmentTimes = null;
        public float[] BestSegmentTimes = null;

        public override void Save(GameDataWriter writer)
        {
            writer.Write(-CurrentFormatVersion);

            writer.Write(PBTotalTime);

            writer.Write(PBSplitTimes.Length);
            for(int i = 0; i < PBSplitTimes.Length; ++i)
            {
                writer.Write(PBSplitTimes[i]);
            }

            writer.Write(PBSegmentTimes.Length);
            for (int i = 0; i < PBSegmentTimes.Length; ++i)
            {
                writer.Write(PBSegmentTimes[i]);
            }

            writer.Write(BestSegmentTimes.Length);
            for (int i = 0; i < BestSegmentTimes.Length; ++i)
            {
                writer.Write(BestSegmentTimes[i]);
            }
        }

        protected override void Init(GameDataReader reader)
        {
            FileVersion = -reader.ReadInt();

            if (CurrentFormatVersion == FileVersion)
            {
                PBTotalTime = reader.ReadFloat();

                PBSplitTimes = new float[reader.ReadInt()];
                for (int i = 0; i < PBSplitTimes.Length; ++i)
                {
                    PBSplitTimes[i] = reader.ReadFloat();
                }

                PBSegmentTimes = new float[reader.ReadInt()];
                for (int i = 0; i < PBSegmentTimes.Length; ++i)
                {
                    PBSegmentTimes[i] = reader.ReadFloat();
                }

                BestSegmentTimes = new float[reader.ReadInt()];
                for (int i = 0; i < BestSegmentTimes.Length; ++i)
                {
                    BestSegmentTimes[i] = reader.ReadFloat();
                }
            }
        }

        public TimerData() { }
    }

    private void SaveData()
    {
        TimerData data = new TimerData();

        if (_hasLoadedTimes)
        {
            if (_totalTime < _pbTime)
            {
                data.PBTotalTime = _totalTime;

                data.PBSegmentTimes = new float[_checkpointCount + 1];
                for (int i = 0; i < _checkpointCount + 1; ++i)
                {
                    data.PBSegmentTimes[i] = _currentSegmentTimes[i];
                }

                data.PBSplitTimes = new float[_checkpointCount + 1];
                for (int i = 0; i < _checkpointCount + 1; ++i)
                {
                    data.PBSplitTimes[i] = _currentSplitTimes[i];
                }
            }
            else
            {
                data.PBTotalTime = _pbTime;

                data.PBSegmentTimes = new float[_checkpointCount + 1];
                for (int i = 0; i < _checkpointCount + 1; ++i)
                {
                    data.PBSegmentTimes[i] = _pbSegmentTimes[i];
                }

                data.PBSplitTimes = new float[_checkpointCount + 1];
                for (int i = 0; i < _checkpointCount + 1; ++i)
                {
                    data.PBSplitTimes[i] = _pbSplitTimes[i];
                }
            }

            data.BestSegmentTimes = new float[_checkpointCount + 1];
            for (int i = 0; i < _checkpointCount + 1; ++i)
            {
                data.BestSegmentTimes[i] = Mathf.Min(_currentSegmentTimes[i], _bestSegmentTimes[i]);
            }
        }
        else
        {
            data.PBTotalTime = _totalTime;

            data.PBSegmentTimes = new float[_checkpointCount + 1];
            for (int i = 0; i < _checkpointCount + 1; ++i)
            {
                data.PBSegmentTimes[i] = _currentSegmentTimes[i];
            }

            data.PBSplitTimes = new float[_checkpointCount + 1];
            for (int i = 0; i < _checkpointCount + 1; ++i)
            {
                data.PBSplitTimes[i] = _currentSplitTimes[i];
            }

            data.BestSegmentTimes = new float[_checkpointCount + 1];
            for (int i = 0; i < _checkpointCount + 1; ++i)
            {
                data.BestSegmentTimes[i] = _currentSegmentTimes[i];
            }
        }

        GameDataWriter.SaveData(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " times", data);
    }

    private void LoadData()
    {
        TimerData data = GameDataReader.LoadData<TimerData>(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " times");

        if ((null != data) && (TimerData.CurrentFormatVersion == data.FileVersion))
        {
            _hasLoadedTimes = true;

            _pbTime = data.PBTotalTime;

            _pbSplitTimes = new float[data.PBSplitTimes.Length];
            _hasLoadedTimes = _hasLoadedTimes && (_pbSplitTimes.Length == _checkpointCount + 1);
            for (int i = 0; i < _pbSplitTimes.Length; ++i)
            {
                _pbSplitTimes[i] = data.PBSplitTimes[i];
            }

            _pbSegmentTimes = new float[data.PBSegmentTimes.Length];
            _hasLoadedTimes = _hasLoadedTimes && (_pbSegmentTimes.Length == _checkpointCount + 1);
            for (int i = 0; i < _pbSegmentTimes.Length; ++i)
            {
                _pbSegmentTimes[i] = data.PBSegmentTimes[i];
            }

            _bestSegmentTimes = new float[data.BestSegmentTimes.Length];
            _hasLoadedTimes = _hasLoadedTimes && (_bestSegmentTimes.Length == _checkpointCount + 1);
            for (int i = 0; i < _bestSegmentTimes.Length; ++i)
            {
                _bestSegmentTimes[i] = data.BestSegmentTimes[i];
            }
        }
    }

    #endregion

    #region Function Overrides

    protected override bool ShouldSurviveSceneChange()
    {
        return false;
    }

    #endregion

    #region Private Variables

    private bool _validTimes = true;

    [SerializeField]
    private bool _running = false;

    [SerializeField]
    private float _totalTime = 0f;

    [SerializeField]
    private float _currentSegmentTime = 0f;

    [SerializeField]
    private float _remainingTime = 0f;

    [SerializeField]
    private float _pbTime = 0f;

    [SerializeField]
    private float[] _currentSplitTimes = null;

    [SerializeField]
    private float[] _currentSegmentTimes = null;

    [SerializeField]
    private float[] _pbSplitTimes = null;

    [SerializeField]
    private float[] _pbSegmentTimes = null;

    [SerializeField]
    private float[] _bestSegmentTimes = null;

    [SerializeField]
    private bool _hasLoadedTimes = false;

    [SerializeField]
    private int _currentSegment = 0;

    [SerializeField]
    private int _checkpointCount = 0;

    private float _unscaledLastTime = 0f;

    #endregion

    #region Public Properties

    public bool LevelIsFinished
    {
        get { return _currentSegment == (_checkpointCount + 1); }
    }

    public float TotalTime
    {
        get { return _totalTime; }
    }

    public float CurrentSegmentTime
    {
        get { return _currentSegmentTime; }
    }

    public float PBTime
    {
        get { return (_hasLoadedTimes ? _pbTime : float.NaN); }
    }

    public float SumOfBestTime
    {
        get
        {
            float sum = float.NaN;

            if (_hasLoadedTimes)
            {
                sum = 0f;

                foreach (var bestSegmentTime in _bestSegmentTimes)
                {
                    sum += bestSegmentTime;
                }
            }

            return sum;
        }
    }

    public float PBSegmentTime
    {
        get { return ((_hasLoadedTimes && _currentSegment <= _checkpointCount + 1) ? _pbSegmentTimes[(_currentSegment <= _checkpointCount) ? _currentSegment : _checkpointCount] : float.NaN); }
    }

    public float BestSegmentTime
    {
        get { return ((_hasLoadedTimes && _currentSegment <= _checkpointCount + 1) ? _bestSegmentTimes[(_currentSegment <= _checkpointCount) ? _currentSegment : _checkpointCount] : float.NaN); }
    }

    public float RemainingTime
    {
        get { return _remainingTime; }
    }

    public bool HasLoadedTime
    {
        get { return _hasLoadedTimes; }
    }

    #endregion

    #region MonoBehaviour Functions

    private void FixedUpdate()
    {
        if (_running)
        {
            float unscaledDeltaTime = Time.fixedUnscaledTime - _unscaledLastTime;

            _totalTime += unscaledDeltaTime;
            _currentSegmentTime += unscaledDeltaTime;

            _unscaledLastTime = Time.unscaledTime;
        }
    }

    #endregion

    #region Public Functions

    public void StartTimers()
    {
        _checkpointCount = CheckPointManager.Instance.CheckpointCount;

        _currentSplitTimes = new float[_checkpointCount + 1];

        for(int i = 0; i < _checkpointCount + 1; ++i)
        {
            _currentSplitTimes[i] = 0f;
        }

        _currentSegmentTimes = new float[_checkpointCount + 1];

        for (int i = 0; i < _checkpointCount + 1; ++i)
        {
            _currentSegmentTimes[i] = 0f;
        }

        _totalTime = 0f;
        _currentSegmentTime = 0f;

        LoadData();

        _running = true;
        _unscaledLastTime = Time.unscaledTime;
    }

    public void UnPause()
    {
        _unscaledLastTime = Time.unscaledTime;
    }

    public void CheckpointReached(int checkpointID)
    {
        SetTimes(checkpointID);
        _currentSegmentTime = 0f;
        _currentSegment = checkpointID + 1;
    }

    public float GetSplitTime(int idx)
    {
        float splitTime = 0f;

        if (idx == _currentSegment)
        {
            splitTime = _totalTime;
        }
        else if ((null != _currentSplitTimes) && (idx < _currentSplitTimes.Length))
        {
            splitTime = _currentSplitTimes[idx];
        }

        return splitTime;
    }

    public float GetSplitDiffTime(int idx)
    {
        float splitDiffTime = float.NaN;

        if (_hasLoadedTimes)
        {
            if (idx < _currentSegment)
            {
                splitDiffTime = _currentSplitTimes[idx] - _pbSplitTimes[idx];
            }
        }

        return splitDiffTime;
    }

    public float GetSegmentTime(int idx)
    {
        float segmentTime = 0f;

        if (idx == _currentSegment)
        {
            segmentTime = _currentSegmentTime;
        }
        else if ((null != _currentSegmentTimes) && (idx < _currentSegmentTimes.Length))
        {
            segmentTime = _currentSegmentTimes[idx];
        }

        return segmentTime;
    }

    public float GetSegmentDiffTime(int idx)
    {
        float segmentDiffTime = float.NaN;

        if (_hasLoadedTimes)
        {
            if (idx < _currentSegment)
            {
                segmentDiffTime = _currentSegmentTimes[idx] - _pbSegmentTimes[idx];
            }
        }

        return segmentDiffTime;
    }

    public Color SplitColor(int idx)
    {
        Color splitColor = Color.white;

        if (!_hasLoadedTimes)
        {
            splitColor = Color.yellow;
        }
        else if (idx < _currentSegment)
        {
            if (_currentSegmentTimes[idx] < _bestSegmentTimes[idx])
            {
                splitColor = Color.yellow;
            }
            else
            {
                splitColor = (GetSplitDiffTime(idx) < 0f) ? Color.green : Color.red;
            }
        }

        return splitColor;
    }

    public void LevelFinished()
    {
        SetTimes(_checkpointCount);
        _currentSegment = _checkpointCount + 1;
        _running = false;

        if (_validTimes)
        {
            SaveData();
        }

        // TODO : Change to actual level finished menu
        GameObject.Find("TempFinish").GetComponent<PauseMenu>().Pausing();
    }

    public void InvalidateTimes()
    {
        _validTimes = false;
    }

    #endregion

    #region Private Functions

    private void SetTimes(int idx)
    {
        for(int i = _currentSegment; i <= idx; ++i)
        {
            _currentSplitTimes[i] = _totalTime;
            _currentSegmentTimes[i] = _currentSegmentTime;
        }
    }

    #endregion
}
