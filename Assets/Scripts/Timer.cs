using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Text timer = null;

    [SerializeField]
    private bool total = true;

    private void Update()
    {
        float time = total ? TimerManager.Instance.TotalTime : TimerManager.Instance.SegmentTime;

        timer.text = ((((((int)time) / 60)) < 10) ? "0" : "") + (((int)time) / 60).ToString() + ":" + (((((int)time) % 60) < 10) ? "0" : "")
            + (((int)time) % 60).ToString() + "." + (((((int)(time * 100f)) % 100) < 10) ? "0" : "" ) + (((int)(time * 100f)) % 100).ToString();
    }
}
