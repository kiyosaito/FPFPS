using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Text timer = null;

    private float time = 0f;

    private bool running = true;

    private void Update()
    {
        if (running)
        {
            time += Time.deltaTime;

            timer.text = ((((((int)time) / 60)) < 10) ? "0" : "") + (((int)time) / 60).ToString() + ":" + (((((int)time) % 60) < 10) ? "0" : "")
                + (((int)time) % 60).ToString() + "." + (((((int)(time * 100f)) % 100) < 10) ? "0" : "" ) + (((int)(time * 100f)) % 100).ToString();
        }
    }

    public void Stop()
    {
        running = false;
    }
}
