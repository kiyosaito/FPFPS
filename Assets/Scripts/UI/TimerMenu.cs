using UnityEngine;

public class TimerMenu : MonoBehaviour
{
    private bool _initialized = false;

    [SerializeField]
    private GameObject _mainTimerArea = null;

    [SerializeField]
    private GameObject _splitTimerArea = null;

    [SerializeField]
    private GameObject _segmentTimerArea = null;

    [SerializeField]
    private GameObject _comparisonTimerArea = null;

    [SerializeField]
    private SplitTimeMenu _splitTimeMenu = null;

    [SerializeField]
    private MonospacedText _currentToatlTime = null;

    [SerializeField]
    private MonospacedText _currentSegmentTime = null;

    [SerializeField]
    private MonospacedText _pbSegmentTime = null;

    [SerializeField]
    private MonospacedText _bestSegmentTime = null;

    [SerializeField]
    private MonospacedText _pbTotalTime = null;

    [SerializeField]
    private MonospacedText _sumOfBestTime = null;

    public void Init()
    {
        if (GameManager.Instance.ShowTimer)
        {
            _mainTimerArea.SetActive(true);
            _segmentTimerArea.SetActive(GameManager.Instance.ShowSegmentTime && TimerManager.Instance.HasLoadedTime);
            _comparisonTimerArea.SetActive(GameManager.Instance.ShowTotalTimeComparison && TimerManager.Instance.HasLoadedTime);

            if (GameManager.Instance.ShowSplits)
            {
                _splitTimerArea.SetActive(true);
                _splitTimeMenu.Init();
            }

            _initialized = true;
        }
    }

    private void Update()
    {
        if (_initialized)
        {
            _currentToatlTime.Text = TimeToString(TimerManager.Instance.TotalTime);
            _currentSegmentTime.Text = TimeToString(TimerManager.Instance.CurrentSegmentTime);
            _pbSegmentTime.Text = TimeToString(TimerManager.Instance.PBSegmentTime);
            _bestSegmentTime.Text = TimeToString(TimerManager.Instance.BestSegmentTime);
            _pbTotalTime.Text = TimeToString(TimerManager.Instance.PBTime);
            _sumOfBestTime.Text = TimeToString(TimerManager.Instance.SumOfBestTime);
        }
    }

    public static string TimeToString(float time, bool prefixed = false)
    {
        string timeString = "";

        if (!float.IsNaN(time))
        {
            string prefix = "";

            if (prefixed)
            {
                prefix = (time < 0) ? "-" : "+";
            }
            time = Mathf.Abs(time);

            if (((((int)time) / 60)) > 99)
            {
                timeString = "99:99.99";
            }
            else
            {
                timeString = prefix + ((((((int)time) / 60)) < 10) ? "0" : "") + (((int)time) / 60).ToString() + ":" + (((((int)time) % 60) < 10) ? "0" : "")
                + (((int)time) % 60).ToString() + "." + (((((int)(time * 100f)) % 100) < 10) ? "0" : "") + (((int)(time * 100f)) % 100).ToString();
            }
        }
        
        return timeString;
    }
}
