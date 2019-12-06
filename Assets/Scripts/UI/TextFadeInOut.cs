using UnityEngine.Events;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextFadeInOut : MonoBehaviour
{
    [SerializeField]
    private float _fadeInTime = 1f;

    [SerializeField]
    private float _fadeOutTime = 1f;

    [SerializeField]
    private UnityEvent _onFadeInDone = default;

    [SerializeField]
    private UnityEvent _onFadeOutDone = default;

    private TextMeshProUGUI _text = null;

    private float _timer = 0f;

    private enum FadeState
    {
        Idle,
        FadeIn,
        FadeOut,
    }

    [SerializeField]
    private FadeState _fadeState = FadeState.Idle;

    public void FadeIn()
    {
        if (FadeState.Idle == _fadeState)
        {
            _timer = _fadeInTime;
        }
        else if (FadeState.FadeOut == _fadeState)
        {
            _timer = (1f - (_timer / _fadeOutTime)) * _fadeInTime;
        }
        _fadeState = FadeState.FadeIn;
    }

    public void FadeOut()
    {
        if (FadeState.Idle == _fadeState)
        {
            _timer = _fadeOutTime;
        }
        else if (FadeState.FadeIn == _fadeState)
        {
            _timer = (1f - (_timer / _fadeInTime)) * _fadeOutTime;
        }
        _fadeState = FadeState.FadeOut;
    }

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);

        if (FadeState.FadeIn == _fadeState)
        {
            _timer = _fadeInTime;
        }
        else if (FadeState.FadeOut == _fadeState)
        {
            _timer = _fadeOutTime;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1f);
        }
    }

    private void Update()
    {
        if (FadeState.FadeIn == _fadeState)
        {
            _timer = Mathf.Max(0f, _timer - Time.unscaledDeltaTime);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1f - (_timer / _fadeInTime));

            if (0f == _timer)
            {
                _fadeState = FadeState.Idle;
                _onFadeInDone.Invoke();
            }
        }
        else if (FadeState.FadeOut == _fadeState)
        {
            _timer = Mathf.Max(0f, _timer - Time.unscaledDeltaTime);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, (_timer / _fadeOutTime));

            if (0f == _timer)
            {
                _fadeState = FadeState.Idle;
                _onFadeOutDone.Invoke();
            }
        }
    }
}
