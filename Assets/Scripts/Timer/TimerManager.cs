using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : UnitySingleton<TimerManager>
{
    #region TimerData

    private class TimerData : PersistableData
    {
        public float TotalTime = 0f;
        public float[] SegmentTimes = null;

        public override void Save(GameDataWriter writer)
        {
            writer.Write(TotalTime);
            writer.Write(SegmentTimes.Length);
            for(int i = 0; i < SegmentTimes.Length; ++i)
            {
                writer.Write(SegmentTimes[i]);
            }
        }

        protected override void Init(GameDataReader reader)
        {
            TotalTime = reader.ReadFloat();
            SegmentTimes = new float[reader.ReadInt()];
            for (int i = 0; i < SegmentTimes.Length; ++i)
            {
                SegmentTimes[i] = reader.ReadFloat();
            }
        }

        public TimerData() { }
    }

    private void SaveData()
    {
        TimerData data = new TimerData();
        data.TotalTime = _totalTime;
        data.SegmentTimes = new float[_finishedSplitTimes.Length];
        for (int i = 0; i < _finishedSplitTimes.Length; ++i)
        {
            data.SegmentTimes[i] = _finishedSplitTimes[i];
        }

        GameDataWriter.SaveData(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " times", data);
    }

    private void LoadData()
    {
        TimerData data = GameDataReader.LoadData<TimerData>(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " times");

        if (null != data)
        {
            _hasLoadedTimes = true;

            _previousSplitTimes = new float[data.SegmentTimes.Length];
            for (int i = 0; i < _previousSplitTimes.Length; ++i)
            {
                _previousSplitTimes[i] = data.SegmentTimes[i];
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

    private bool _running = true;

    private float _totalTime = 0f;

    private float _currentSegmentTime = 0f;

    private float _remainingTime = 0f;

    private float[] _finishedSplitTimes = null;

    private float[] _previousSplitTimes = null;

    private bool _hasLoadedTimes = false;

    private int _currentSegment = 0;

    #endregion

    #region Public Properties

    public float TotalTime
    {
        get { return _totalTime; }
    }

    public float CurrentSegmentTime
    {
        get { return _currentSegmentTime; }
    }

    public float RemainingTime
    {
        get { return _remainingTime; }
    }

    #endregion

    #region MonoBehaviour Functions

    private void Update()
    {
        if (_running)
        {
            _totalTime += Time.deltaTime;
            _currentSegmentTime += Time.deltaTime;
        }
    }

    #endregion

    #region Public Functions

    public void StartTimers()
    {
        int checkpointCount = CheckPointManager.Instance.CheckpointCount;

        _finishedSplitTimes = new float[checkpointCount + 1];

        for(int i = 0; i < checkpointCount; ++i)
        {
            _finishedSplitTimes[i] = 0f;
        }

        _previousSplitTimes = new float[checkpointCount + 1];

        for (int i = 0; i < checkpointCount; ++i)
        {
            _previousSplitTimes[i] = 0f;
        }

        _totalTime = 0f;
        _currentSegmentTime = 0f;

        LoadData();
    }

    public void CheckpointReached(int checkpointID)
    {
        _finishedSplitTimes[checkpointID] = _totalTime;
        _currentSegmentTime = 0f;
        _currentSegment = checkpointID + 1;
    }

    public float GetSplitTime(int idx)
    {
        float segmentTime = 0f;

        if (idx == _currentSegment)
        {
            segmentTime = _totalTime;
        }
        else if (null != _finishedSplitTimes)
        {
            segmentTime = _finishedSplitTimes[idx];
        }

        return segmentTime;
    }

    public float GetSplitDiffTime(int idx)
    {
        float segmentDiffTime = float.NaN;

        if (_hasLoadedTimes)
        {
            if (idx < _currentSegment)
            {
                segmentDiffTime = _finishedSplitTimes[idx] - _previousSplitTimes[idx];
            }
        }

        return segmentDiffTime;
    }

    public void LevelFinished()
    {
        _finishedSplitTimes[_finishedSplitTimes.Length - 1] = _totalTime;
        _currentSegment = _finishedSplitTimes.Length;
        _running = false;

        SaveData();
    }

    #endregion
}
