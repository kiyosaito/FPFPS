using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : UnitySingleton<TimerManager>
{
    #region Function Overrides

    protected override bool ShouldSurviveSceneChange()
    {
        return false;
    }

    #endregion

    #region Private Variables

    private float _totalTime = 0f;

    private float _segmentTime = 0f;

    private float _remainingTime = 0f;

    private float[] _finishedSegmentTimes = null;

    private float[] _previousSegmentTimes = null;

    #endregion

    #region Public Properties

    public float TotalTime
    {
        get { return _totalTime; }
    }

    public float SegmentTime
    {
        get { return _segmentTime; }
    }

    public float RemainingTime
    {
        get { return _remainingTime; }
    }

    #endregion

    #region MonoBehaviour Functions

    private void Update()
    {
        _totalTime += Time.deltaTime;
        _segmentTime += Time.deltaTime;
    }

    #endregion

    #region Public Functions

    public void StartTimers()
    {
        int checkpointCount = CheckPointManager.Instance.CheckpointCount;

        _finishedSegmentTimes = new float[checkpointCount + 1];

        for(int i = 0; i < checkpointCount; ++i)
        {
            _finishedSegmentTimes[i] = 0f;
        }

        _previousSegmentTimes = new float[checkpointCount + 1];

        for (int i = 0; i < checkpointCount; ++i)
        {
            _previousSegmentTimes[i] = 0f;
        }

        _totalTime = 0f;
        _segmentTime = 0f;
    }

    public void CheckpointReached(int checkpointID)
    {
        _finishedSegmentTimes[checkpointID] = _segmentTime;
        _segmentTime = 0f;
    }

    public void LevelFinished()
    {
        _finishedSegmentTimes[_finishedSegmentTimes.Length - 1] = _segmentTime;
    }

    #endregion
}
