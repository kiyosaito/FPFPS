using UnityEngine;
using TMPro;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class MonospacedText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProText = null;

    private TextMeshProUGUI TMPText
    {
        get
        {
            if (null == _textMeshProText)
            {
                _textMeshProText = GetComponent<TextMeshProUGUI>();
            }

            return _textMeshProText;
        }
    }

    [SerializeField]
    private float _ratio = 1f;

    public string Text { set { TMPText.text = "<mspace=" + _textMeshProText.fontSize * _ratio + ">" + value + "</mspace>"; } }

    public Color TextColor { set { TMPText.color = value; } }
}
