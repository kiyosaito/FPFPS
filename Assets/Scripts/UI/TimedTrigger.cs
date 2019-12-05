using UnityEngine.Events;
using UnityEngine;


public class TimedTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _timedTrigger = default;

    [SerializeField]
    private bool _running = false;

    [SerializeField]
    private float _time = 1f;

    private float _timer = 0f;

    void Start()
    {
        _timer = _running ? _time : 0f;
    }

    void Update()
    {
        if (_running)
        {
            _timer = Mathf.Max(0f, _timer - Time.unscaledDeltaTime);

            if (0f == _timer)
            {
                _running = false;
                _timedTrigger.Invoke();
            }
        }
    }

    public void TurnOn()
    {
        if (!_running)
        {
            _running = true;
            _timer = _time;
        }
    }

    public void TurnOff()
    {
        if (_running)
        {
            _running = false;
            _timer = 0f;
        }
    }

    public void ReStartTimer()
    {
        _running = true;
        _timer = _time;
    }

    public void TriggerNow()
    {
        _running = false;
        _timer = 0f;
        _timedTrigger.Invoke();
    }
}
