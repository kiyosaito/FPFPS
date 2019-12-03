using UnityEngine;

public class SplitTimeDisplay : MonoBehaviour
{
    [SerializeField]
    private MonospacedText _splitTime = null;

    [SerializeField]
    private MonospacedText _splitDiffTime = null;

    public string SplitTime { set { _splitTime.Text = value; } }

    public string SplitDiffTime { set { _splitDiffTime.Text = value; } }

    public Color DiffColor { set { _splitDiffTime.TextColor = value; } }
}
