using System.Collections.Generic;
using UnityEngine;

public class SplitTimeMenu : MonoBehaviour
{
    private bool _initialized = false;

    [SerializeField]
    private GameObject _splitTimeDisplayPrefab = null;

    private List<SplitTimeDisplay> _splitTimes = null;

    public void Init()
    {
        int checkpointCount = CheckPointManager.Instance.CheckpointCount;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 40f * checkpointCount);
        _splitTimes = new List<SplitTimeDisplay>();

        for (int i = 0; i < checkpointCount; ++i)
        {
            GameObject splitDisplayObj = Instantiate<GameObject>(_splitTimeDisplayPrefab, transform);
            RectTransform splitRectTransform = splitDisplayObj.GetComponent<RectTransform>();
            splitRectTransform.anchoredPosition = new Vector2(splitRectTransform.anchoredPosition.x, -40f * i);
            _splitTimes.Add(splitDisplayObj.GetComponent<SplitTimeDisplay>());
        }

        _initialized = true;
    }

    public void Update()
    {
        if (_initialized)
        {
            for (int i = 0; i < _splitTimes.Count; ++i)
            {
                _splitTimes[i].SplitTime = TimerMenu.TimeToString(TimerManager.Instance.GetSplitTime(i + 1));
                _splitTimes[i].SplitDiffTime = TimerMenu.TimeToString(TimerManager.Instance.GetSplitDiffTime(i + 1), true);
                _splitTimes[i].DiffColor = TimerManager.Instance.SplitColor(i + 1);
            }
        }
    }
}
