using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Text timer = null;

    [SerializeField]
    private bool total = true;

    [SerializeField]
    private bool diff = false;

    [SerializeField]
    private int segment = 1;

    [SerializeField]
    private bool best = false;

    private void Update()
    {
        float time = total ? TimerManager.Instance.TotalTime : (diff ? TimerManager.Instance.GetSplitDiffTime(segment) : TimerManager.Instance.GetSplitTime(segment));
        time = best ? TimerManager.Instance.GetSumOfBestSegments() : time;

        if (float.IsNaN(time))
        {
            timer.text = "";
        }
        else
        {
            string prefix = "";

            if (!total && diff)
            {
                prefix = (time < 0) ? "-" : "+";
                time = Mathf.Abs(time);
            }

            timer.text = prefix + ((((((int)time) / 60)) < 10) ? "0" : "") + (((int)time) / 60).ToString() + ":" + (((((int)time) % 60) < 10) ? "0" : "")
                + (((int)time) % 60).ToString() + "." + (((((int)(time * 100f)) % 100) < 10) ? "0" : "") + (((int)(time * 100f)) % 100).ToString();
        }
    }
}
