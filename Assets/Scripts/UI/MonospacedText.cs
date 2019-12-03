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
    private string _text = "";

    [SerializeField]
    private Color _textColor = Color.white;

    [SerializeField]
    private float _ratio = 1f;

    /*private void Update()
    {
        Text = _text;
        TextColor = _textColor;
    }*/

    public string Text { set { TMPText.text = "<mspace=" + _textMeshProText.fontSize * _ratio + ">" + value + "</mspace>"; } }

    public Color TextColor { set { TMPText.color = value; } }
}
